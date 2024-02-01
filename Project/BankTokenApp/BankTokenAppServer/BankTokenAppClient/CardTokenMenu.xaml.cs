﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BankTokenAppClient
{
    /// <summary>
    /// Interaction logic for CardTokenMenu.xaml
    /// </summary>
    public partial class CardTokenMenu : UserControl
    {
        private Client? client;
        private string? creditCardNumber;
        private string? tokenNumber;

        public event PropertyChangedEventHandler? PropertyChanged;
        public static readonly DependencyProperty IsValidBankCardProperty =
            DependencyProperty.Register("IsValidBankCard", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsValidTokenProperty =
            DependencyProperty.Register("IsValidToken", typeof(bool), typeof(UserControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsCardRegisteredProperty =
            DependencyProperty.Register("IsCardRegistered", typeof(bool), typeof(UserControl), new PropertyMetadata(false));

        public CardTokenMenu()
        {
            InitializeComponent();
            DataContext = this;
            client = null;
            creditCardNumber = null;
            tokenNumber = null;
        }

        public string CreditCardNumber
        {
            get { return creditCardNumber; }
            set
            {
                creditCardNumber = value;
                OnPropertyChanged(nameof(CreditCardNumber));
            }
        }

        public string TokenNumber
        {
            get { return tokenNumber; }
            set
            {
                tokenNumber = value;
                //IsValidBankCard = IsValidToken(tokenNumber);
                OnPropertyChanged(nameof(TokenNumber));
            }
        }

        public bool IsValidBankCard
        {
            get { return (bool)GetValue(IsValidBankCardProperty); }
            set { SetValue(IsValidBankCardProperty, value); }
        }

        public bool IsValidToken
        {
            get { return (bool)GetValue(IsValidTokenProperty); }
            set { SetValue(IsValidTokenProperty, value); }
        }

        public bool IsCardRegistered
        {
            get { return (bool)GetValue(IsCardRegisteredProperty); }
            set { SetValue(IsCardRegisteredProperty, value); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void setClient(Client c)
        {
            client = c;
            currentUsername.Content = c.LoggedUser.Username;
            canRegisterLabel.Content = c.LoggedUser.CanRegisterToken ? "Yes" : "No";
            canRetrieveLabel.Content = c.LoggedUser.CanRetrieveCardNumber ? "Yes" : "No";
            getButton.IsEnabled = c.LoggedUser.CanRetrieveCardNumber;
            registerButton.IsEnabled = c.LoggedUser.CanRegisterToken;

            changeLabelColor(canRegisterLabel);
            changeLabelColor(canRetrieveLabel);
        }

        private void changeLabelColor(Label l)
        {
            if ((string)l.Content == "Yes")
            {
                l.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                l.Foreground = new SolidColorBrush(Colors.Red);
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

        private void RegisterCardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tokenTextBox.Text = client.RegisterToken(inputTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in registering token by card number", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetCardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cardNumberOutputTextBox.Text = client.RetrieveCardNumber(tokenInputTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in retrieving card number by token", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //registerMessageLabel.Visibility = Visibility.Hidden;
        }
    }
}
