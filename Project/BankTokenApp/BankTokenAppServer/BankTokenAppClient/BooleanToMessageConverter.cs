using System.Globalization;
using System.Windows.Data;

namespace BankTokenAppClient
{
    public class BooleanToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isValid && isValid)
            {
                return "Valid card!";
            }
            else
            {
                return "Invalid card! Try again.";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
