using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;

namespace BankTokenAppServer
{
    /// <summary>
    /// Main Window for the server side application
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private Server server;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            server = new Server(logTextBox);
        }
        #endregion

        #region WindowClosing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }
        #endregion


        #region ButtonClicks
        /// <summary>
        /// Button click method used to start up the server when clicked. 
        /// Creates a new thread and executes the server.Start method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (server != null)
            {
                Thread serverThread = new Thread(server.Start);

                serverThread.Start();
            }

            StartServerButton.IsEnabled = false;
            logTextBox.Text += "Server is listening for incoming connections...\n";
        }

        /// <summary>
        /// Button click method used to export existing Card-Token relationship to a file
        /// in the format Token-CardNumber, where the data is sorted by the Token
        /// Opens a SaveFileDialog and lets the user pick a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportByTokenButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text files (*.txt)|*.txt";

            saveFileDialog.Title = "Save a text file";

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;
                if (server != null)
                {
                    bool status = server.ExportTokensByToken(fileName);
                    if (status)
                    {
                        MessageBox.Show($"Relations Token - Card Number were successfully written to file {fileName}", "Writing to file", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"There was an error writing relations Token - Card Number to file", "Error writing to file", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Button click method used to export existing Card-Token relationship to a file
        /// in the format CardNumber-Token, where the data is sorted by the CardNumber
        /// Opens a SaveFileDialog and lets the user pick a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportByCardButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text files (*.txt)|*.txt";

            saveFileDialog.Title = "Save a text file";

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;
                if (server != null)
                {
                    bool status = server.ExportTokensByCard(fileName);
                    if (status)
                    {
                        MessageBox.Show($"Relations Card Number - Token were successfully written to file {fileName}", "Writing to file", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"There was an error writing relations Card Number - Token to file", "Error writing to file", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion
    }
}