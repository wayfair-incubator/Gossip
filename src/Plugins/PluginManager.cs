using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gossip.UnitTests")]
namespace Gossip.Plugins
{
    /// <inheritdoc cref="IPluginManager"/>
    public class PluginManager : IPluginManager
    {
        private readonly List<(Func<IDatabasePlugin> factory, Type type)> _pluginFactories;

        public PluginManager()
        {
            _pluginFactories = new List<(Func<IDatabasePlugin> factory, Type type)>();
        }

        public void AddPlugin<T>(Func<T> pluginFactory) where T : IDatabasePlugin
        {
            _pluginFactories.Add((() => pluginFactory(), typeof(T)));
        }

        public IEnumerable<IDatabasePlugin> InstantiatePlugins()
        {
            var plugins = new List<IDatabasePlugin>();

            foreach (var (factory, type) in _pluginFactories)
            {
                IDatabasePlugin plugin;
                try
                {
                    plugin = factory();
                }
                catch (Exception e)
                {
                    throw new PluginInstantiationException(type, e);
                }

                if (plugin is null)
                {
                    throw new NullPluginException(type);
                }

                plugins.Add(plugin);
            }

            return plugins;
        }
    }
}