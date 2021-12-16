using System;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Plugins
{
    [ExcludeFromCodeCoverage]
    public class NullPluginException : Exception
    {
        public Type PluginType { get; }

        public NullPluginException(Type pluginType) : base($"Plugin factory returned null. Plugin type: {pluginType.Name}")
        {
            PluginType = pluginType;
        }
    }
}