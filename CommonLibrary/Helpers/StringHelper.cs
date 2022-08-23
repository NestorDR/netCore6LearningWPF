using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Call this method to generate a random string, for example, like a password
        /// </summary>
        /// <param name="length">How long to make the resulting string</param>
        /// <returns>A random set of values</returns>
        public static string CreateRandomString(int length)
        {
            const string CHAR_LIST = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@";
            StringBuilder sb = new(32);

            using (RNGCryptoServiceProvider csp = new())
            {
                var buffer = new byte[sizeof(uint)];

                for (int i = length; i > 0; i--)
                {
                    csp.GetBytes(buffer);
                    var number = BitConverter.ToUInt32(buffer, 0);
                    sb.Append(CHAR_LIST[(int)(number % (uint)CHAR_LIST.Length)]);
                }
            }

            return sb.ToString();
        }

    }
}
