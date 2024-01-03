using System.Windows;

namespace CodeChangeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CodeChangeLib.CodeGeneratorWPF codeGeneratorWPFInstance;

        public MainWindow()
        {
            InitializeComponent();

            codeGeneratorWPFInstance = codeGeneratorWPF;
            codeGeneratorWPFInstance.CodeChange += CodeChangeHandler;
        }

        private void CodeChangeHandler(object sender, CodeChangeLib.CodeChangeEventArgs e)
        {
            AddGeneratedCodesToTextBox(e.Code);
        }

        private void AddGeneratedCodesToTextBox(List<int> codes)
        {
            codes.Sort();
            codesTextBox.Text += string.Join(" ", codes);
        }

        private void CodeStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (codesTextBox.Text != "")
            {
                List<int> allCodes = codesTextBox.Text.Split(' ').Select(int.Parse).ToList();

                var groupedCodes = allCodes.GroupBy(code => code / 100)
                                           .OrderByDescending(group => group.Key);
                foreach (var group in groupedCodes)
                {
                    string spaces = group.Key == 9 ? "  " : "    ";
                    codeStatisticsTextBox.Text += $"Codes between {group.Key * 100} and {(group.Key + 1) * 100}{spaces}have    {group.Distinct().Count()} distinct numbers\n";
                }
                codeStatisticsTextBox.Text += $"\nTotal numbers in statistics {allCodes.Count}\n";
            }
        }
    }
}