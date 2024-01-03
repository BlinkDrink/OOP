using System.Windows;
using System.Windows.Controls;

namespace CodeChangeLib
{
    /// <summary>
    /// Interaction logic for CodeGeneratorWPF.xaml
    /// </summary>
    public partial class CodeGeneratorWPF : UserControl
    {
        public event EventHandler<CodeChangeEventArgs> CodeChange;

        public CodeGeneratorWPF()
        {
            InitializeComponent();
        }

        private void ClearInput_Click(object sender, RoutedEventArgs e)
        {
            GeneratedCodeTextBox.Clear();
        }

        // Event handler за натискане на бутона "Generate Random Code"
        private void GenerateRandomCodeButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> numbers = GenerateRandomNumbers();
            DisplayNumbers(numbers);

            // Ако има абонати за събитието CodeChange, изпращаме информацията
            if (CodeChange != null)
            {
                CodeChangeEventArgs eventArgs = new CodeChangeEventArgs(numbers);
                OnCodeChange(eventArgs);
            }
        }

        private List<int> GenerateRandomNumbers()
        {
            Random random = new Random();
            List<int> numbers = new List<int>();

            for (int i = 0; i < 60; i++)
            {
                int number = random.Next(100, 1000 - 1);
                if (number % 10 == 0)
                {
                    numbers.Add(number);
                }
                else
                {
                    i--;
                }
            }
            return numbers;
        }

        // Показване на числата в текстовото поле
        private void DisplayNumbers(List<int> numbers)
        {
            GeneratedCodeTextBox.Text = string.Join(" ", numbers);
        }

        // Метод за извикване на събитието CodeChange
        protected virtual void OnCodeChange(CodeChangeEventArgs e)
        {
            CodeChange?.Invoke(this, e);
        }

        // Event handler за натискане на бутона "Quit"
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
