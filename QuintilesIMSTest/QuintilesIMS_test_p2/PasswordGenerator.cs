using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuintilesIMS_test_p2
{
    static class PasswordGenerator
    {
        private static readonly Random rng = new Random();
        /// <summary>
        /// Create a new password which meets password complexity rules
        /// </summary>
        /// <returns></returns>
        public static string GetPassword(string userName, string email)
        {
            const string allowedChars =
                "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~@#$%^&*+=|<>/\\!’?”-:,;()[]{}";
            bool flag = false;
            string result = String.Empty;
            //create a random string until it pass the passwordValidator
            while (!flag)
            {
                result = RandomStrings(allowedChars, 8, 16, rng);
                flag = PasswordValidator.IsValid(userName, email, result);
            }
            return result;
        }

        /// <summary>
        /// Generate a random string with length restriction in the given chars set
        /// </summary>
        /// <param name="allowedChars"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="rng"></param>
        /// <returns></returns>
        private static string RandomStrings(string allowedChars,int minLength, int maxLength, Random rng)
        {
            int pwdLength = rng.Next(minLength, maxLength+1);
            char[] chars = new char[pwdLength];
            int setLength = allowedChars.Length;

            for (int i = 0; i < pwdLength; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }
            return new string(chars);
        }
    }
}
