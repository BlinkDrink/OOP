using System.Windows;
using System.Windows.Controls;

namespace CalculatorSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double firstInputNumber;
        private double secondInputNumber;
        private double result;
        private Operation operation;

        public enum Operation
        {
            ADDITION,
            SUBTRACTION,
            MULTIPLICATION,
            DIVISION,
            NO_OPERATION,
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Number button handler
        /// </summary>
        /// <param name="sender">The caller of the Click function for the button click</param>
        /// <param name="e"></param>
        private void NumberButton_Clicked(object sender, RoutedEventArgs e)
        {
            var buttonText = ((Button)sender).Content.ToString();

            if (txtInput.Text.Contains(".") && buttonText == ".")
            {
                return;
            }
            else
            {
                txtInput.Text += buttonText;
            }
        }

        private void OperationButton_Clicked(object sender, RoutedEventArgs e)
        {
            // Parse the currently active number in the text box input for our firstInputNumber
            if (!double.TryParse(txtInput.Text, out firstInputNumber))
            {
                MessageBox.Show("Invalid number!");
            }

            var buttonText = ((Button)sender).Content.ToString();

            // Determine the operation the user has clicked
            operation = buttonText switch
            {
                "+" => Operation.ADDITION,
                "-" => Operation.SUBTRACTION,
                "x" => Operation.MULTIPLICATION,
                "/" => Operation.DIVISION,
                _ => Operation.NO_OPERATION
            };

            txtInput.Text = "0";
        }

        /// <summary>
        /// Equality button handler
        /// Used for computing the equation given by the user
        /// </summary>
        /// <param name="sender">The object calling the ComputeButton_Click method</param>
        /// <param name="e"></param>
        private void ComputeButton_Click(object sender, RoutedEventArgs e)
        {
            // Parse the currently active number in the text box input for our secondInputNumber
            if (!double.TryParse(txtInput.Text, out secondInputNumber))
            {
                MessageBox.Show("Invalid number!");
            }

            // Determine the operation which we should perform then compute it
            result = operation switch
            {
                Operation.ADDITION => firstInputNumber + secondInputNumber,
                Operation.SUBTRACTION => firstInputNumber - secondInputNumber,
                Operation.MULTIPLICATION => firstInputNumber * secondInputNumber,
                Operation.DIVISION => firstInputNumber / secondInputNumber,
                _ => 0,
            };

            txtInput.Text = $"{result}";
        }

        /// <summary>
        /// Clears the text input field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "0";
        }

        /// <summary>
        /// Clears the text input field and all the memorized variables up till now
        /// </summary>
        /// <param name="sender">The object that called ClearAllButton_Click</param>
        /// <param name="e"></param>
        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            firstInputNumber = 0;
            secondInputNumber = 0;
            operation = Operation.NO_OPERATION;
            txtInput.Text = "0";
        }
    }
}
