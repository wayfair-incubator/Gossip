using System;

namespace Gossip.Plugins
{
    public class PluginInstantiationException : Exception
    {
        public Type PluginType { get; }

        public PluginInstantiationException(Type pluginType, Exception innerException) : base($"Plugin could not be instantiated. Plugin type: {pluginType?.Name}", innerException)
        {
            PluginType = pluginType;
        }
    }
}