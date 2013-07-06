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
    public partial class PostAdSuccessPage : PhoneApplicationPage
    {
        public PostAdSuccessPage()
        {
            InitializeComponent();
        }

        private void ShareAppBarButton_Click(object sender, EventArgs e)
        {

        }

        private void OkAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
    }
}