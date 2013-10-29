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
using System.Windows.Input;

namespace cipele46.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel();
        }

        private void SettingsAppBarButton_Click(object sender, EventArgs e)
        {
            //used to update update the binding source for textboxes
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
            (this.DataContext as SettingsViewModel).ChangeUserData();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ChangePasswordPage.xaml", UriKind.Relative));
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).User = null;
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }
    }
}