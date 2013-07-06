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
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
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

        private void TryCreate()
        {
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

            }
        }
    }
}