using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cipele46.Views
{
    public partial class RegisterPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public RegisterPage()
        {
            InitializeComponent();

            DataContext = this;

#if DEBUG
            NameTextBox.Text = "Testni user" + new Random().Next();

            EmailTextBox.Text = new Random().Next().ToString() + "user@gigimail.com";
            PhoneTextBox.Text = "0981234567";

            PasswordTextBox.Password = "sifra111";
            ConfirmPasswordTextBox.Password = "sifra111";
#endif
        }

        private void RegisterAppBarButton_Click(object sender, EventArgs e)
        {
            TryCreate();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (sender == NameTextBox)
                    EmailTextBox.Focus();
                else if (sender == EmailTextBox)
                    PhoneTextBox.Focus();
                else if (sender == PhoneTextBox)
                    PasswordTextBox.Focus();
                else if (sender == PasswordTextBox)
                    ConfirmPasswordTextBox.Focus();
                else if (sender == ConfirmPasswordTextBox)
                {
                    TryCreate();
                }
            }
        }

        private async Task TryCreate()
        {
            if (RegisterAppBarButton != null)
                RegisterAppBarButton.IsEnabled = false;
            IsBusy = true;

            String email = EmailTextBox.Text;
            String name = NameTextBox.Text;
            String phone = PhoneTextBox.Text;
            String password = PasswordTextBox.Password;
            String confirmPassword = ConfirmPasswordTextBox.Password;

            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(ErrorStrings.EmailEmpty);
            }
            else if (String.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(ErrorStrings.NameEmpty);
            }
            else if (String.IsNullOrWhiteSpace(phone))
            {
                //no phone validation
            }
            else if (String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(ErrorStrings.PasswordEmpty);
            }
            else if (String.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show(ErrorStrings.ConfirmPasswordWrong);
            }
            else
            {
                // try it!
                // curl -v
                // -H "Accept: application/json"
                // -H "Content-type: application/json"
                // -X POST
                // -d ' {"user":{"first_name":"firstname","last_name":"lastname","email":"emai333331231233l@email.com","password":"app123","password_confirmation":"app123"}}'
                // http://cipele46.org/users.json

                var request = WebRequest.CreateHttp("http://cipele46.org/users.json");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                var data = JsonConvert.SerializeObject(new registration_info
                {
                    user = new user
                    {
                        //first_name = "firstname",
                        //last_name = "lastname",
                        email = EmailTextBox.Text,
                        password = PasswordTextBox.Password,
                        password_confirmation = ConfirmPasswordTextBox.Password
                    }
                });
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // Prepare web request...
                var newStream = await request.GetRequestStreamAsync();
                // Send the data.
                newStream.Write(byteData, 0, byteData.Length);
                newStream.Close();

                var response = await request.GetResponseAsync() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var user = new Model.user
                    {
                        name = NameTextBox.Text,
                        password = PasswordTextBox.Password,
                        email = EmailTextBox.Text
                    };

                    ((App)Application.Current).User = user;

                    // try logging in immediately
                    request = HttpWebRequest.CreateHttp("http://cipele46.org/users/show.json");
                    request.Method = "GET";
                    request.Accept = "application/json";
                    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
                    response = await request.GetResponseAsync() as HttpWebResponse;
                    if (response.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Uspješno ste kreirali korisnički račun");
                }

                if (RegisterAppBarButton != null)
                    RegisterAppBarButton.IsEnabled = true;
                IsBusy = false;
            }
        }

        public class user
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string password_confirmation { get; set; }
        }

        public class registration_info
        {
            public user user { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}