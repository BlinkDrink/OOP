using System.Globalization;
using System.Windows.Controls;

namespace BankTokenAppClient
{
    /// <summary>
    /// Simple class used to validate Bank Card Number format
    /// </summary>
    public class BankCardValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? s = value?.ToString();
            if (IsCreditCardValid(s))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Invalid bank card number");
            }

        }
        public static bool IsCreditCardValid(string creditCardNumber)
        {
            int sum = 0;
            bool alternate = false;

            if (creditCardNumber.Length != 16)
                return false;

            for (int i = creditCardNumber.Length - 1; i >= 0; i--)
            {
                int digit = creditCardNumber[i] - '0';

                if (alternate)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }
                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }
    }
}
