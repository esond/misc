using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Flamtap.Extensions;

namespace Flamtap.Serialization
{
    public static class SerializationHelper
    {
        public static IEnumerable<string> ToCsv<T>(IEnumerable<T> objects, string separator = ",", bool header = true)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            PropertyInfo[] properties = typeof(T).GetProperties();

            if (header)
            {
                yield return string.Join(separator, fields.Select(f => f.Name.ToDisplayText())
                    .Concat(properties.Select(p => p.Name.ToDisplayText())).ToArray());
            }

            foreach (T obj in objects)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(obj) ?? "").ToString())
                    .Concat(properties.Select(p => (p.GetValue(obj, null) ?? "").ToString())).ToArray());
            }
        }
    }
}
