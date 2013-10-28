using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using cipele46.ViewModels;

namespace cipele46.Views
{
    public partial class NewAdPage : PhoneApplicationPage
    {        
        public NewAdPage()
        {
            InitializeComponent();
            this.DataContext = App.NewAdViewModel;
            if (((App)Application.Current).User != null)
            {
                App.NewAdViewModel.Email = ((App)Application.Current).User.email;
                App.NewAdViewModel.Phone = ((App)Application.Current).User.phone;
            }
        }

        private void NewAdAppBarButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(App.NewAdViewModel.Title))
            {
            }

            if (String.IsNullOrWhiteSpace(App.NewAdViewModel.Description))
            {
            }

            App.NewAdViewModel.PostAnAd();

            NavigationService.Navigate(new Uri("/Views/PostAdSuccessPage.xaml", UriKind.Relative));
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}