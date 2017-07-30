using System.Collections.Generic;

namespace Flamtap.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Wraps this object instance into an IEnumerable&lt;T&gt; containing the object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns> An IEnumerable&lt;T&gt; containing a single item.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            if (item == null) yield break;

            yield return item;
        }
    }
}
