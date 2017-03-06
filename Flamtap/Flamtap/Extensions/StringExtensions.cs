using System.Collections.Generic;
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

        /// <summary>
        /// Splits a string into substrings based on the characters in an array, but keeps the characters in the 
        /// substring, unlike string.Split().
        /// </summary>
        /// <param name="s">The string to split.</param>
        /// <param name="separator">A character that delimits the substrings in this string.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitAndKeep(this string s, char separator)
        {
            IEnumerable<string> tokens = s.Split(separator);

            foreach (string token in tokens)
                yield return $"{separator}{token}";
        }
    }
}
