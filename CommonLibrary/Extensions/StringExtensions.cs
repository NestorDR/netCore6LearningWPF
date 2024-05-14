using System.Globalization;
using System.Text.RegularExpressions;

namespace CommonLibrary.Extensions
{
    /// <summary>
    /// Visit https://www.codeproject.com/articles/692603/csharp-string-extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalizes the first character of the string while keeping the rest the same
        /// </summary>
        public static string CapitalizeFirst(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s)) return string.Empty;

            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s[1..];
        }
        
        /// <summary>
        ///  Gets the words of the string that are in the list.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<string> Intersect(this string s, string[] list)
        {
            string[] sourceList = s.Split(' ');
            IEnumerable<string> words = sourceList.Intersect(list, StringComparer.OrdinalIgnoreCase);
            return words;
        }

        public static DateTime ToDate(this string s, bool throwExceptionIfFailed = false)
        {
            var valid = DateTime.TryParse(s, out var result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException($"'{s}' cannot be converted as DateTime");
            return result;
        }

        public static double ToDouble(this string s, bool throwExceptionIfFailed = false, double defaultValue = 0)
        {
            var valid = double.TryParse(s, NumberStyles.AllowDecimalPoint,
                new NumberFormatInfo { NumberDecimalSeparator = "." }, out double result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException($"'{s}' cannot be converted as double");
            return defaultValue;
        }

        public static int ToInt(this string s, bool throwExceptionIfFailed = false, int defaultValue = 0)
        {
            var valid = int.TryParse(s, out int result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException($"'{s}' cannot be converted as int");
            return defaultValue;
        }

        /// <summary>
        /// Matching all capital letters of the string and separate them with spaces to form a sentence.
        /// If the string is an abbreviation text, no space will be added and returns the same string.
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>A string with a space added  before each capital letter, but not the first one</returns>
        public static string ToSentence(this string s)
        {
            s = s.ToUpper()[0] + s[1..];        // range indexer: s[1..] is equal to s.Substring(1)

            if (string.IsNullOrWhiteSpace(s))
                return s;
            // return as is if the s is just an abbreviation
            if (Regex.Match(s, "[0-9A-Z]+$").Success)
                return s;
            // add a space before each capital letter, but not the first one.
            string result = Regex.Replace(s, "(\\B[A-Z])", " $1");
            return result;
        }

        public static string Reverse(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Returns the last howMany characters of the string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="howMany"></param>
        /// <returns></returns>
        public static string GetLast(this string s, int howMany)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var value = s.Trim();
            return howMany >= value.Length ? value : value.Substring(value.Length - howMany);
        }

        /// <summary>
        /// Returns the first howMany characters of the string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="howMany"></param>
        /// <returns></returns>
        public static string GetFirst(this string s, int howMany)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var value = s.Trim();
            return howMany >= value.Length ? value : s[..howMany];  // without range indexer Substring(0, howMany)
        }

        /// <summary>
        /// Pass in a string and this method will determine if it is all lower case characters or not.
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>True if the string passed in is all lower case, otherwise false.</returns>
        public static bool IsAllLowerCase(this string s)
        {
            Match match = Regex.Match(s, @"^([^A-Z])+$");
            return match.Success;
        }

        /// <summary>
        /// Pass in a string and this method will determine if it is all upper case characters or not.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>True if the string passed in is all upper case, otherwise false.</returns>
        public static bool IsAllUpperCase(this string s)
        {
            Match match = Regex.Match(s, @"^([^a-z])+$");
            return match.Success;
        }

        public static bool IsEmail(this string s)
        {
            Match match = Regex.Match(s,
                @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-*)|(\w+\.))*\w+\.[a-zA-Z]{2,6}$",
                RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static bool IsNumber(this string s)
        {
            Match match = Regex.Match(s, @"^[0-9]+$", RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static bool IsPhone(this string s)
        {
            Match match = Regex.Match(s, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static string ExtractEmail(this string? s)
        {
            if (s == null || string.IsNullOrWhiteSpace(s)) return string.Empty;

            Match match = Regex.Match(s, @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-*)|(\w+\.))*\w+\.[a-zA-Z]{2,6}$", RegexOptions.IgnoreCase);
            return match.Success ? match.Value : string.Empty;
        }

        public static int ExtractNumber(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            Match match = Regex.Match(s, "[0-9]+", RegexOptions.IgnoreCase);
            return match.Success ? match.Value.ToInt() : 0;
        }

        public static string? ExtractQueryStringParamValue(this string queryString, string paramName)
        {
            if (string.IsNullOrWhiteSpace(queryString) || string.IsNullOrWhiteSpace(paramName)) return string.Empty;

            var query = queryString.Replace("?", "");
            if (!query.Contains($"=")) return string.Empty;
            Dictionary<string, string> queryValues = query.Split('&').Select(piQ => piQ.Split('=')).ToDictionary(piKey => piKey[0].ToLower().Trim(), piValue => piValue[1]);
            bool found = queryValues.TryGetValue(paramName.ToLower().Trim(), out var result);
            return found ? result : string.Empty;

        }

        /// <summary>
        /// The Levenshtein's Distance Algorithm compares two strings approximately.
        /// This algorithm calculates the minimum number of single-character edits (insertions, deletions, or substitutions) required to change one string into the other.
        /// The smaller the number of edits, the more similar the two strings are.
        /// </summary>
        public static int LevenshteinDistance(this string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;

            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            int levenshteinDistance = d[n, m];
            
            return levenshteinDistance;
        }
    }
}