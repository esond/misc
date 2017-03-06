using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string[] SplitUnixArgs(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new string[0];

            int start = 0;

            List<string> args = new List<string>();

            if (!s.StartsWith("-"))
            {
                start = s.IndexOf(" ", StringComparison.Ordinal);
                args.Add(s.Substring(0, start)); // the verb, if any
            }

            int index = s.IndexOf("-", start, StringComparison.Ordinal);

            while (index != -1)
            {
                int indexOfNextCommand = s.IndexOf(" -", index + 1, StringComparison.Ordinal);

                args.Add(indexOfNextCommand != -1
                    ? s.Substring(index, indexOfNextCommand)
                    : s.Substring(index, s.Length - index));

                index = indexOfNextCommand;
            }

            return args.Select(a => a.Trim()).ToArray();
        }
    }
}
