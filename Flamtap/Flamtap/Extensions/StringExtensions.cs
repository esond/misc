using System.Text.RegularExpressions;

namespace Flamtap.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a camel-case string into individual words.
        /// (e.g. "HomerSimpson".ToDisplayName() == "Homer Simpson")
        /// </summary>
        /// <param name="value">The value.</param>
        public static string ToDisplayText(this string value)
        {
            return Regex.Replace(value, @"([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}
