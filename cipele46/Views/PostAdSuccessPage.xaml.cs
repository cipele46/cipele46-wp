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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string editAd;
            if (e.NavigationMode == NavigationMode.New
                && NavigationContext.QueryString.TryGetValue("editAd", out editAd))
            {
                if (editAd == "true")
                {
                    PageTitle.Text = "uredi oglas";
                }
            }
        }

        private void ShareAppBarButton_Click(object sender, EventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        private void OkAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
    }
}