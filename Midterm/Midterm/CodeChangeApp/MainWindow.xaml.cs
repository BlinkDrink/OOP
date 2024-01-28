using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CodeChangeApp
{
    public partial class MainWindow : Window
    {
        private CodeChangeLib.CodeGenerator codeGeneratorWPFInstance; // instance of the UserControl for the code generation

        public MainWindow()
        {
            InitializeComponent();

            codeGeneratorWPFInstance = codeGenerator;
            codeGeneratorWPFInstance.CodeChange += CodeChangeHandler;
        }

        #region EventSubscribingMethods
        /// <summary>
        /// Custom handler used to reflect the changes made in the TextBox of 
        /// CodeChangeLib 
        /// </summary>
        /// <param name="sender">The object calling this method</param>
        /// <param name="e">CodeChangeEventArgs containing the string codes</param>
        private void CodeChangeHandler(object sender, CodeChangeLib.CodeChangeEventArgs e)
        {
            AddGeneratedCodesToTextBox(e.Codes);
        } 
        #endregion

        #region HelperMethods
        /// <summary>
        /// Helper method used to display the generated codes into the text box 
        /// </summary>
        /// <param name="codes">List of two-letered strings</param>
        private void AddGeneratedCodesToTextBox(List<string> codes)
        {
            if (codesTextBox.Text == "")
                codesTextBox.Text += string.Join(" ", codes);
            else
                codesTextBox.Text += " " + string.Join(" ", codes);
        }

        /// <summary>
        /// Helper method used to check if a pair of letters are in the format $"{LetterCode1}{LetterCode1+1}"
        /// </summary>
        /// <param name="code">The pair of letters to check</param>
        /// <returns>True if the condition is fulfilled, false otherwise</returns>
        private bool isSecondLeterAfterFirst(string code)
        {
            if (code.Length != 2)
                return false;

            if (code[0] == 'Я' && code[1] == 'А')
                return true;

            return code[1] - code[0] == 1;
        }
        #endregion

        #region ButtonClickMethods
        /// <summary>
        /// Handles the Code Statistics button click
        /// </summary>
        private void CodeStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (codesTextBox.Text != "")
            {
                var allPairs = codesTextBox.Text.Split(' ').ToList();
                var group = allPairs.Where(code => isSecondLeterAfterFirst(code) == true).GroupBy(x => x)
                 .ToDictionary(g => g.Key, g => g.Count());
                StringBuilder sb = new StringBuilder();
                foreach (var pair in group)
                {
                    sb.Append($"{pair.Key}\t{pair.Value}\n");
                }
                sb.Append("End of statistics for this click\n");
                codeStatisticsTextBox.Text += sb.ToString();
            }
        } 
        #endregion
    }
}
