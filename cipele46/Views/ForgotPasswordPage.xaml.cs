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
    public partial class ForgotPasswordPage : PhoneApplicationPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void ForgotPasswordAppBarButton_Click(object sender, EventArgs e)
        {
            String email = EmailTextBox.Text;
            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Molimo unesite svoj e-mail");
            }
            else
            {
            }
        }
    }
}