using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuintilesIMS_test_p2
{
    static class PasswordValidator
    {
        public static ICollection<String> ErrorMessage { get; private set; }

        public static bool IsValid(string userName, string email,string password)
        {
            bool flag = true;
            ErrorMessage = new List<string>();
            if (!CheckLength(password))
            {
                flag = false;
                ErrorMessage.Add("Password must be at least 8 chars");
            }
            if (!CheckUserNameTokens(userName, password))
            {
                flag = false;
                ErrorMessage.Add("Password cannot contain user's entire name or tokens");
            }
            if (!CheckEmailTokens(email, password))
            {
                flag = false;
                ErrorMessage.Add("Password cannot contain email address's local part or domain part");
            }
            if (!CheckCharSets(password))
            {
                flag = false;
                ErrorMessage.Add("Password must contain at least 3 of 5 chars sets");
            }
            return flag;
        }

        private static bool CheckLength(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            return true;
        }
        private static bool CheckUserNameTokens(string userName, string password)
        {
            //Check user's entire name and tokens
            Char[] userNamedelimiter = new Char[] { ',', '.', '-', '_', ' ', '#', '\t' };
            String[] userNameTokens = userName.Split(userNamedelimiter, StringSplitOptions.RemoveEmptyEntries);
            //iteratively check if the password contains the tokens (longer than 2) in username, case insensive
            foreach (var token in userNameTokens)
            {
                if (token.Length >= 3)
                {
                    if (password.ToLower().Contains(token.ToLower()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private static bool CheckEmailTokens(string email,string password)
        {
            //Check email's local part and domain part 
            char emailDelimiter = '@';
            String[] emailTokens = email.Split(emailDelimiter);
            //iteratively check if the password contains the tokens in email, case insensive
            foreach (var token in emailTokens)
            {
                if (password.ToLower().Contains(token.ToLower()))
                {
                    return false;                }
            }
            return true;

        }
        private static bool CheckCharSets(string password)
        {
            //must contain at least 3 of 5 chars sets
            int conditionsCount = 0;
            HashSet<char> punctuationChars = new HashSet<char>() { '!', '\'', '?', '"', '-', ':', ',', ';', '(', ')', '[', ']', '{', '}' };
            HashSet<char> symbolsChars = new HashSet<char>() { '~', '@', '#', '$', '%', '^', '&', '*', '+', '=', '|', '<', '>', '/', '\\' };

            if (password.Any(Char.IsLower))
            {
                conditionsCount++;
            }
               
            if (password.Any(Char.IsUpper))
            {
                conditionsCount++;
            }
                
            if (password.Any(Char.IsDigit))
            {
                conditionsCount++;
            }
               
            if (password.Any(punctuationChars.Contains))
            {
                conditionsCount++;
            }
                
            if (password.Any(symbolsChars.Contains))
            {
                conditionsCount++;
            }

            if (conditionsCount < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
