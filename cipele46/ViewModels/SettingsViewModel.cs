using cipele46.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

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
