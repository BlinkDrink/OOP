using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;

namespace BankTokenAppServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server;

        public MainWindow()
        {
            InitializeComponent();
            server = new Server(logTextBox);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

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
    }
}