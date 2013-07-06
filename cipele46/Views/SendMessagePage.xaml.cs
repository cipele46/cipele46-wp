using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using cipele46.Model;

namespace cipele46.Views
{
    public partial class SendMessagePage : PhoneApplicationPage
    {
        private ViewModels.AdViewModel _viewModel;

        public SendMessagePage()
        {
            InitializeComponent();
            DataContext = _viewModel = App.SelectedAd;
            this.Loaded += SendMessagePage_Loaded;
        }

        private void SendMessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            user user = ((App)Application.Current).User;
            if (user != null && !String.IsNullOrWhiteSpace(user.email))
            {
                EmailTextBox.Text = user.email;
            }
        }

        private void SendMessageAppbarButton_Click(object sender, EventArgs e)
        {
            String email = EmailTextBox.Text;
            String message = MessageTextBox.Text;

            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(ErrorStrings.EmailEmpty);
            }
            else if (String.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(ErrorStrings.MessageEmpty);
            }
            else
            {

            }
        }
    }
}