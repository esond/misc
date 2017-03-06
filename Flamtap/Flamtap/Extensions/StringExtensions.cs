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
        /// Splits up a string of unix-style arguments into an array of arguments.
        /// </summary>
        /// <param name="s">The arguments string to parse.</param>
        /// <returns></returns>
        public static string[] SplitUnixArgs(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new string[0];

            List<string> args = new List<string>();

            int start = 0, length = 0;

            if (!s.StartsWith("-")) //the first token is a verb
            {
                start = s.IndexOf(" ", StringComparison.Ordinal);

                if (start == -1)
                    return s.Yield().ToArray();

                args.Add(s.Substring(0, start));
            }

            while (length != -1)
            {
                length = s.IndexOf(" -", start + 1, StringComparison.Ordinal);

                string toAdd = length == -1 
                    ? s.Substring(start, s.Length - start) 
                    : s.Substring(start, length - start);

                start += toAdd.Length;

                if (!string.IsNullOrWhiteSpace(toAdd))
                    args.Add(toAdd.Trim());
            }

            return args.ToArray();
        }
    }
}
