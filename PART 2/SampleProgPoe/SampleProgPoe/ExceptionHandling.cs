using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleProgPoe
{
    public class ExceptionHandling
    {

        public int NumValid(string input, string fieldName)
        {
            int num;
            if (int.TryParse(input, out num))
            {
                return num;
            }
            string errorMessage = $"Please Provide Number only for : {fieldName}";
            throw new ArgumentException(errorMessage);

        }

        public int NumValid1(string input)
        {
            int num;
            if (int.TryParse(input, out num))
            {
                return num;
            }
            string errorMessage = $"Please Provide Number only for : {input}";
            throw new ArgumentException(errorMessage);

        }

        public string ModuleCode(string code)
        {
            string pattern = "^[A-Za-z]{1,}[0-9]+$";

            if (!Regex.IsMatch(code, pattern))
            {
                string errorMessage = $"Please provide a Letter for Module Code such as 'PROG6212 with capital Letter'";
                throw new ArgumentException(errorMessage);
            }

            return code;
        }

        public string DateHandling(string input)
        {
            if (!DateTime.TryParseExact(input, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                string errorMessage = $"Please provide the date in the format 'yyyy/MM/dd', such as '2023/06/27'";
                throw new ArgumentException(errorMessage);
            }
            return input;
        }

        public string StringValid(string input, string fieldName)
        {

            if (string.IsNullOrEmpty(input))
            {
                string errorMessage = $"Please don't leave any empty space for : {fieldName}";
                throw new ArgumentException(errorMessage);
            }

            return input;

        }
        public string PasswordHanlding(string input, string fieldname)
        {
            if (string.IsNullOrEmpty(input))
            {
                string errorMessage = $"Please don't leave any empty space for : {fieldname}";
                throw new ArgumentException(errorMessage);
            }
            else if (input.Length != 10)
            {
                string errorMessage = $"Please make sure password must be 10 characters : {fieldname}";
                throw new ArgumentException(errorMessage);
            }
            else if (!char.IsUpper(input[0]))
            {
                string errorMessage = $"Please make sure first character must be an upper case Letter : {fieldname}";
                throw new ArgumentException(errorMessage);
            }
            else if (!input.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                string errorMessage = $"Please make sure to include at least one special character in your password : {fieldname}";
                throw new ArgumentException(errorMessage);
            }
            else
            {
                for (int v = 0; v < input.Length; v++)
                {
                    if (!char.IsLetterOrDigit(input[v]) && !char.IsPunctuation(input[v]))
                    {
                        string errorMessage = $"Please make sure that rest must be numbers, letters or special characters : {fieldname}";
                        throw new ArgumentException(errorMessage);
                    }
                }
            }
            return input;
        }
    }
}
