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
using System.Text;
using Newtonsoft.Json;
using cipele46.ViewModels;

namespace cipele46.Views
{
    public partial class ChangePasswordPage : PhoneApplicationPage
    {
        private user newUser;

        public ChangePasswordPage()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel();
        }

        private void ChangePasswordAppBarButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(PasswordTextBox.Password) 
                || String.IsNullOrWhiteSpace(NewPasswordTextBox.Password)
                || String.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Password))
            {
                MessageBox.Show(ErrorStrings.PasswordEmpty);
                return;
            }

            if (!NewPasswordTextBox.Password.Equals(ConfirmPasswordTextBox.Password))
            {
                MessageBox.Show(ErrorStrings.ConfirmPasswordWrong);
                return;
            }

            (this.DataContext as SettingsViewModel).IsDataLoading = true;
            (this.DataContext as SettingsViewModel).IsDataLoaded = false;
            user user = ((App)Application.Current).User;
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
            wc.UploadStringCompleted += wc_UploadStringCompleted;

            newUser = new user
            {
                first_name = user.first_name,
                last_name = user.last_name,
                phone = user.phone,
                email = user.email,
                password = NewPasswordTextBox.Password,
                password_confirmation = ConfirmPasswordTextBox.Password
            };

            var data = JsonConvert.SerializeObject(new Tools.registration_info
            {
                user = newUser
            });

            wc.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString();
            wc.UploadStringAsync(new Uri(Endpoints.LoginUserUrl), "PUT", data);
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ((App)Application.Current).User = newUser;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    (App.Current.RootVisual as PhoneApplicationFrame).GoBack();
                });
            }
            else
            {
                MessageBox.Show(ErrorStrings.LoginFail);
            }
            (this.DataContext as SettingsViewModel).IsDataLoading = false;
            (this.DataContext as SettingsViewModel).IsDataLoaded = true;
        }
    }
}