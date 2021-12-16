using System;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Plugins
{
    [ExcludeFromCodeCoverage]
    public class PluginInstantiationException : Exception
    {
        public Type PluginType { get; }

        public PluginInstantiationException(Type pluginType, Exception innerException) : base($"Plugin could not be instantiated. Plugin type: {pluginType?.Name}", innerException)
        {
            PluginType = pluginType;
        }
    }
}