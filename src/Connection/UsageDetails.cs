using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Gossip.Connection
{
    [ExcludeFromCodeCoverage]
    public class UsageDetails
    {
        public string LibraryName { get; }
        public Version LibraryVersion { get; }

        public UsageDetails(Assembly assembly)
        {
            var assemblyName = assembly.GetName();
            LibraryName = assemblyName.Name;
            LibraryVersion = assemblyName.Version;
        }
    }
}