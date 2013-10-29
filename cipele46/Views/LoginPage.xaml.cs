using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using cipele46.Model;

namespace cipele46.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {
        string successUri;

        public LoginPage()
        {
            InitializeComponent();
            this.Loaded += LoginPage_Loaded;
        }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("successUri", out successUri);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var user = ((App)App.Current).User;
            if (user != null)
            {
                EmailTextBox.Text = user.email;
                PasswordTextBox.Password = user.password;
            }
        }

        private void fbSigninButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/FacebookLoginPage.xaml", UriKind.Relative));
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            String email = EmailTextBox.Text;
            String password = PasswordTextBox.Password;

            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(ErrorStrings.EmailEmpty);
                return;
            }
            else if (String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(ErrorStrings.PasswordEmpty);
                return;
            }
            else
            {
                user user = new user();
                user.email = email;
                user.password = password;

                HttpStatusCode loginStatusCode = await Tools.LoginUser(user);

                if (loginStatusCode == HttpStatusCode.OK)
                {
                    if (!String.IsNullOrWhiteSpace(successUri))
                    {
                        NavigationService.Navigate(new Uri(successUri, UriKind.Relative));
                        Dispatcher.BeginInvoke(() =>
                        {
                            NavigationService.RemoveBackEntry();
                        });
                    }
                    else
                    {
                        NavigationService.GoBack();
                    }
                }
                else
                {
                    MessageBox.Show(ErrorStrings.LoginFail);
                }
                
            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ForgotPasswordPage.xaml", UriKind.Relative));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/RegisterPage.xaml", UriKind.Relative));
        }
    }
}