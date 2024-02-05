using EducationServiceLib;
using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace EducationClient
{
    public partial class MainWindow : Window
    {
        private TcpClient? tcpClient;
        private NetworkStream? networkStream;
        private BinaryReader? reader;
        private BinaryWriter? writer;

        public MainWindow()
        {
            InitializeComponent();
            Random rand = new Random();

            Title = $"Order Client {rand.Next(1,101)}";
            Thread currentClientThread = new Thread(()=> RunClient());
            currentClientThread.Start();   
        }

        private void RunClient()
        {
            try
            {
                tcpClient = new TcpClient("127.0.0.1", 55555); // За порт съм си избрал 55555

                networkStream = tcpClient.GetStream();
                reader = new BinaryReader(networkStream);
                writer = new BinaryWriter(networkStream);

                string titlesData = reader.ReadString();
                string serviceTypesData = reader.ReadString();

                InitializeComboBoxes(titlesData, serviceTypesData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeComboBoxes(string titlesData, string serviceTypesData)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                {
                    cboTitles.ItemsSource = titlesData.Split(',');
                    cboTypes.ItemsSource = serviceTypesData.Split(',');
                });
            }
            else
            {
                cboTitles.ItemsSource = titlesData.Split(',');
                cboTypes.ItemsSource = serviceTypesData.Split(',');
            }
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tmp, numStudents;
                string title = Title;
                string cboTitle = cboTitles.SelectedItem.ToString();
                ServiceType parsedServiceType;
                Enum.TryParse<ServiceType>(cboTypes.SelectedItem.ToString(), out parsedServiceType);
                numStudents = int.TryParse(studentsQty.Text, out tmp) ? tmp : 0;

                SendOrderToServer(title, cboTitle, parsedServiceType, numStudents);

                //string serverResponse = ReceiveDataFromServer();
                //string[] responseParts = serverResponse.Split(',');

                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    cboTitles.ItemsSource = responseParts[0].Split(','); 
                //    cboTypes.ItemsSource = responseParts[1].Split(',');
                //});
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendOrderToServer(string clientTitle, string title, ServiceType type, int qty)
        {
            writer?.Write($"{clientTitle},{title},{type},{qty}");
        }

        //private string ReceiveDataFromServer()
        //{
        //    string response = reader.ReadString();
        //    return response;
        //}


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            studentsQty.Text = "0";
        }
    }
}
