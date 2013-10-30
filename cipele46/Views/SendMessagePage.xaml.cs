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
using Newtonsoft.Json;
using System.Text;

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

        private async void SendMessageAppbarButton_Click(object sender, EventArgs e)
        {
            String email = EmailTextBox.Text;
            String message = MessageTextBox.Text;

            if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(ErrorStrings.EmailEmpty);
                EmailTextBox.Focus();
            }
            else if (String.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(ErrorStrings.MessageEmpty);
                MessageTextBox.Focus();
            }
            else
            {
                this.Focus();

                _viewModel.IsDataLoaded = false;
                _viewModel.IsDataLoading = true;

                var request = WebRequest.CreateHttp(String.Format(Endpoints.ReplyToAddUrl, _viewModel.Id));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                var data = JsonConvert.SerializeObject(new ReplyToAd
                {
                    email = email,
                    content = message
                });
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                var newStream = await request.GetRequestStreamAsync();
                newStream.Write(byteData, 0, byteData.Length);
                newStream.Close();

                try
                {
                    var response = await request.GetResponseAsync() as HttpWebResponse;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //MessageBox?
                    }
                }
                catch (Exception ex)
                {
                    //Error message?
                }

                _viewModel.IsDataLoaded = true;
                _viewModel.IsDataLoading = false;
            }
        }

        private class ReplyToAd
        {
            public string email { get; set; }
            public string content { get; set; }
        }
    }
}