using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using cipele46.Model;
using System.Windows;
using System.Net;
using System.Text;
using Microsoft.Phone.Shell;

namespace cipele46.Views
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        private ViewModels.AdViewModel _viewModel;

        public DetailsPage()
        {
            InitializeComponent();
            DataContext = _viewModel = App.SelectedAd;
        }

        private void CallAppBarButton_Click(object sender, System.EventArgs e)
        {            
            _viewModel.CallPhoneCommand.Execute(null);
        }

        private void MessageAppBarButton_Click(object sender, System.EventArgs e)
        {
            //user user = ((App)Application.Current).User;
            Uri sendMessageUri = new Uri("/Views/SendMessagePage.xaml", UriKind.Relative);
            //if (user == null)
            //{
            //    MessageBoxResult result = MessageBox.Show("Za slanje poruke potrebna je prijava", "Prijavite se", MessageBoxButton.OKCancel);
            //    if (result.Equals(MessageBoxResult.OK))
            //    {
            //        NavigationService.Navigate(new Uri(String.Format("/Views/LoginPage.xaml?successUri={0}", sendMessageUri.OriginalString), UriKind.Relative));
            //    }               
            //}
            //else
            //{
                NavigationService.Navigate(sendMessageUri);
            //}
        }

        private void FavoriteAppBarButton_Click(object sender, System.EventArgs e)
        {
            user user = ((App)Application.Current).User;
            if (user == null)
            {
                MessageBoxResult result = MessageBox.Show("Za dodavanje oglasa u favorite potrebno se prijaviti", "Prijavite se", MessageBoxButton.OKCancel);
                if (result.Equals(MessageBoxResult.OK))
                {
                    NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
                }
            }
            else
            {
                _viewModel.IsDataLoading = true;
                _viewModel.IsDataLoaded = false;
                user = ((App)Application.Current).User;
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.Headers[HttpRequestHeader.ContentLength] = "0";
                wc.UploadStringAsync(new Uri(String.Format(Endpoints.ToggleFavoriteUrl, _viewModel.Id)), "PUT", "");
            }
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[2];

                if (btn.IconUri.ToString().Contains("appbar.favs.addto.rest.png"))
                {
                    btn.IconUri = new Uri("Icons/appbar.favs.rest.png", UriKind.Relative);
                }
                else
                {
                    btn.IconUri = new Uri("Icons/appbar.favs.addto.rest.png", UriKind.Relative);
                }
            }
            else
            {

            }
            _viewModel.IsDataLoading = false;
            _viewModel.IsDataLoaded = true;
        }
    }
}