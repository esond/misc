using System.Text.RegularExpressions;

namespace Flamtap.Extensions
{
    public static class StringExtensions
    {
        public static string ToDisplayText(this string str)
        {
            return string.Join(" ", Regex.Split(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))"));
        }
    }
}
