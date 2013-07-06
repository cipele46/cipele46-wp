using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace cipele46.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void fbSigninButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/FacebookLoginPage.xaml", UriKind.Relative));
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            String email = EmailTextBox.Text;
            String password = PasswordTextBox.Password;

            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Molimo upišite svoj email i lozinku");
                return;
            }
            else
            {

            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ForgotPasswordPage.xaml", UriKind.Relative));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}