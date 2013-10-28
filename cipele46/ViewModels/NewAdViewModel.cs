using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipele46.ViewModels
{
    public class NewAdViewModel : ViewModelBaseEx
    {
        private String _title;
        private String _description;
        private String _email;
        private String _phone;

        public String Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public String Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        public String Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public String Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        public void PostAnAd()
        {
        }
    }
}
