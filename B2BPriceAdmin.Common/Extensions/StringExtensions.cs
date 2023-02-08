using System.Text;

namespace B2BPriceAdmin.Common.Extensions
{
    /// <summary>
    /// Extension methods for String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// String SafeTrim Extension Method
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string SafeTrim(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;
            return val.Trim();
        }

        /// <summary>
        /// Returns a copy of the original string containing only the set of whitelisted characters.
        /// </summary>
        /// <param name="value">The string that will be copied and scrubbed.</param>
        /// <param name="alphas">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="numerics">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="dashes">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="underlines">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="spaces">If true, all alphabetical characters (a-zA-Z) will be preserved; otherwise, they will be removed.</param>
        /// <param name="periods">If true, all alphabetical characters (.) will be preserved; otherwise, they will be removed.</param>
        /// <param name="symbols">If true, all symbols characters (\/:*?"<>|%) will be preserved; otherwise, they will be removed.</param>
        public static string RemoveExcept(string value, bool alphas = true, bool numerics = true, bool dashes = true, bool underlines = true, bool periods = true, bool spaces = false, bool symbols = false)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (new[] { alphas, numerics, dashes, underlines, spaces, periods, symbols }.All(x => x == false)) return value;

            var whitelistChars = new HashSet<char>(string.Concat(
                alphas ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" : "",
                numerics ? "0123456789" : "",
                dashes ? "-" : "",
                underlines ? "_" : "",
                periods ? "." : "",
                symbols ? "\\/:*?\"<>|%" : "",
                spaces ? " " : ""
            ).ToCharArray());

            var scrubbedValue = value.Aggregate(new StringBuilder(), (sb, @char) =>
            {
                if (whitelistChars.Contains(@char)) sb.Append(@char);
                return sb;
            }).ToString();

            return scrubbedValue;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// Ordering is important.If one of the postFixes is matched, others will not be tested.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="postFixes"></param>
        /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (string text in postFixes)
            {
                if (str.EndsWith(text))
                {
                    return str.Left(str.Length - text.Length);
                }
            }

            return str;
        }

        /// <summary>
        ///  Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns>Modified string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Left(this string str, int length)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < length)
            {
                throw new ArgumentException("length argument can not be bigger than given string's length!");
            }

            return str.Substring(0, length);
        }
    }
}
