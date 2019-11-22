using System.Collections.Generic;

namespace Misc.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Returns an enumerator for this object instance.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns> An IEnumerable&lt;T&gt; containing a single item.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            if (!typeof(T).IsValueType && item == null)
                yield break;

            yield return item;
        }
    }
}
