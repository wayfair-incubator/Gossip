using System;
using System.Collections.Generic;

namespace Gossip.Plugins
{
    /// <summary>
    ///     Manages instantiation of plugins.
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        ///     Add a plugin to the manager.
        /// </summary>
        /// <typeparam name="T">The type of plugin to add.</typeparam>
        /// <param name="pluginFactory">A function returning the plugin.</param>
        void AddPlugin<T>(Func<T> pluginFactory) where T : IDatabasePlugin;

        /// <summary>
        ///     Instantiate the plugins that the manager is managing.
        /// </summary>
        /// <returns>An <see cref="IList{T}"/> of <see cref="IDatabasePlugin"/>.</returns>
        IEnumerable<IDatabasePlugin> InstantiatePlugins();
    }
}