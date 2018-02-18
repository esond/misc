using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Flamtap.Validation;
using Newtonsoft.Json.Linq;

namespace Flamtap.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Returns a string containing all the characters appearing after the last index of a given value.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="indexOf">The string to index from.</param>
        public static string AfterLast(this string value, string indexOf)
        {
            return value.Substring(value.LastIndexOf(indexOf, StringComparison.Ordinal) + indexOf.Length);
        }

        /// <summary>
        ///     Returns a substring containing the characters between, but not including, the specified strings.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="left">The string to get characters after.</param>
        /// <param name="right">The tring to get characters before.</param>
        /// <returns></returns>
        public static string Between(this string value, string left, string right)
        {
            if (!value.Contains(left))
                return string.Empty;

            var afterFirst = value.Split(new[] { left }, StringSplitOptions.None)[1];

            if (!afterFirst.Contains(right))
                return string.Empty;

            var result = afterFirst.Split(new[] { right }, StringSplitOptions.None)[0];

            return result;
        }

        /// <summary>
        ///     Evaluates whether or not a string consists of only ASCII characters.
        /// </summary>
        /// <param name="value">The string to evaluate</param>
        /// <returns>True if the string contains only ASCII characters.</returns>
        public static bool IsAscii(this string value)
        {
            /* It could be argued that IsAscii() should return true when given an empty string, because an empty string
             * isn't *not* ASCII. However, if one is checking a string's contents, they probably meant for the string
             * to hold some kind of value in the first place, so we may as well break. */
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            const int maxAsciiCode = 127;

            return value.All(c => c <= maxAsciiCode);
        }

        /// <summary>
        ///     Determines if a string is a valid Base 64 encoded string or not.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns>True if the string is a valid Base 64 string.</returns>
        public static bool IsBase64(this string value)
        {
            return value.Length % 4 == 0 &&
                   Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        ///     Determines if a string is well-formed JSON.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns>True if the string is well-formed JSON.</returns>
        public static bool IsJson(this string value)
        {
            value = value.Trim();

            return (value.StartsWith("{") && value.EndsWith("}") || value.StartsWith("[") && value.EndsWith("]"))
                && IsWellFormed();

            bool IsWellFormed()
            {
                try
                {
                    JToken.Parse(value);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        ///     Evaluates whether or not a string is in UTF-8 encoding.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the string is a valid UTF-8 string.</returns>
        public static bool IsUtf8(this string value)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                return Utf8Validator.IsUtf8(stream);
            }
        }

        /// <summary>
        ///     Remove all characters from a string that are not a letter or a number.
        /// </summary>
        public static string RemoveNonAlphanumeric(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9 ]", string.Empty);
        }

        /// <summary>
        ///     Like string.Split(), but preserves the separator on the end of the results.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <param name="separator">
        ///     A character array that delimits the substrings in this string, an empty array that contains no
        ///     delimiters, or null.
        /// </param>
        /// <param name="options">
        ///     StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the result or
        ///     System.StringSplitOptions.None to include empty array elements in the result.
        /// </param>
        /// <returns>
        ///     An array whose elements contain the substrings from this instance that are delimited by one or
        ///     more characters in separator.
        /// </returns>
        public static string[] SplitAndKeep(this string value, string separator,
            StringSplitOptions options = StringSplitOptions.None)
        {
            var result = Regex.Split(value, $"(?<=[{separator}])");

            return options == StringSplitOptions.RemoveEmptyEntries
                ? result.Where(s => !string.IsNullOrEmpty(s)).ToArray()
                : result;
        }

        /// <summary>
        ///     Splits up a string of unix-style arguments into an array of arguments.
        /// </summary>
        /// <param name="s">The arguments string to parse.</param>
        /// <returns></returns>
        public static string[] SplitUnixArgs(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return new string[0];

            var args = new List<string>();

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

                var toAdd = length == -1
                    ? s.Substring(start, s.Length - start)
                    : s.Substring(start, length - start);

                start += toAdd.Length;

                if (!string.IsNullOrWhiteSpace(toAdd))
                    args.Add(toAdd.Trim());
            }

            return args.ToArray();
        }

        /// <summary>
        ///     Replace the diacritic characters in in a string with their ASCII equivalents (if possible).
        ///     e.g. "Éric Søndergard".StripDiacritics() == "Eric Sondergard"
        /// </summary>
        /// <param name="value">The value with diacratics.</param>
        /// <returns>The value without diacritics.</returns>
        /// <see cref="http://stackoverflow.com/a/249126/1672990" />
        public static string StripDiacritics(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        ///     Splits a camel-case string into individual words.
        ///     (e.g. "HomerSimpson".ToDisplayName() == "Homer Simpson")
        /// </summary>
        /// <param name="value">The value.</param>
        public static string ToDisplayText(this string value)
        {
            return Regex.Replace(value, @"([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        /// <summary>
        ///     Converts the string to a valid file name by replacing invalid chars with underscores or a given value.
        ///     (e.g. "08/03/2017".ToValidFileName() == "Backup on 08_03_2017")
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="replacement">The string to replace invalid characters with.</param>
        /// <returns>A valid filename.</returns>
        public static string ToValidFileName(this string value, string replacement = "_")
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var invalidChars = Path.GetInvalidFileNameChars();

            if (replacement.ToCharArray().Intersect(invalidChars).Any())
            {
                throw new ArgumentException($"{nameof(replacement)} cannot contain invalid file name characters.",
                    nameof(replacement));
            }

            var result = new StringBuilder();

            foreach (var c in value.StripDiacritics())
            {
                if (' ' <= c && c <= '~' && Array.IndexOf(invalidChars, c) < 0)
                    result.Append(c);
                else
                    result.Append(replacement);
            }

            return result.ToString();
        }
    }
}
