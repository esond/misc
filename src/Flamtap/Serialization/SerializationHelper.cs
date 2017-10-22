using System.Collections.Generic;
using System.Linq;
using Flamtap.Extensions;

namespace Flamtap.Serialization
{
    public static class SerializationHelper
    {
        public static IEnumerable<string> ToCsv<T>(IEnumerable<T> objects, string separator = ",", bool header = true)
        {
            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            if (header)
            {
                yield return string.Join(separator, fields.Select(f => f.Name.ToDisplayText())
                    .Concat(properties.Select(p => p.Name.ToDisplayText())).ToArray());
            }

            foreach (var obj in objects)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(obj) ?? "").ToString())
                    .Concat(properties.Select(p => (p.GetValue(obj, null) ?? "").ToString())).ToArray());
            }
        }
    }
}
