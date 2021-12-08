using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gossip.Utilities
{
    public static class EnumerableExtensions
    {
        public static string ToJsonArray<T>(this IEnumerable<T> items)
        {
            return JsonConvert.SerializeObject(items);
        }
    }
}
