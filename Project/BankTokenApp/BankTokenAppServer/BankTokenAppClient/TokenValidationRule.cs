using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BankTokenAppClient
{
    public class TokenValidationRule : ValidationRule
    {
        private const string TokenPattern = @"^(?![3456])[0-9]{11}[0-9]{4}$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? s = value?.ToString();
            int[] digits = s.Select(c => int.Parse(c.ToString())).ToArray();
            bool sum = digits.Sum() % 10 != 0;

            if (IsTokenValid(s) && sum)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid token format");
            }
        }

        private bool IsTokenValid(string s)
        {
            Regex regex = new Regex(TokenPattern);
            return regex.IsMatch(s);
        }
    }

}
