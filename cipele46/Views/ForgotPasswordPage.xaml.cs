using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace cipele46.Views
{
    public partial class ForgotPasswordPage : PhoneApplicationPage
    {
        private static string forgotPasswordUrl = "http://www.cipele46.org";

        public ForgotPasswordPage()
        {
            InitializeComponent();
            ForgotPasswordMessage.Text = ErrorStrings.ForgotPasswordMessage;
            ForgotPasswordUrl.Text = forgotPasswordUrl;            
        }

        private void ForgotPasswordAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ForgotPasswordUrl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var wbt = new WebBrowserTask();
            wbt.Uri = new Uri(forgotPasswordUrl);
            wbt.Show();
        }
    }
}