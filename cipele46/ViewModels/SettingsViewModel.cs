using cipele46.Model;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace cipele46.ViewModels
{
    public class SettingsViewModel : ViewModelBaseEx
    {
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;
        private string _password;
        private string _confirmPassword;

        private user newUser;

        public SettingsViewModel()
        {
            user user = ((App)Application.Current).User;
            if (user != null)
            {
                FirstName = user.first_name;
                LastName = user.last_name;
                Phone = user.phone;
            }
        }

        public void ChangeUserData()
        {
            user user = ((App)Application.Current).User;
            WebClient wc = new WebClient();            
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
            wc.UploadStringCompleted += wc_UploadStringCompleted;

            newUser = new user
            {
                first_name = FirstName,
                last_name = LastName,
                phone = Phone,
                email = user.email,
                password = user.password
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
            ((App)Application.Current).User = newUser;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                (App.Current.RootVisual as PhoneApplicationFrame).GoBack();
            });
        }

        public String FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public String LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public String Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        public String Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public String Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public String ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }
    }
}
