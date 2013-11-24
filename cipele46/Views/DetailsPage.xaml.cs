using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using cipele46.Model;
using System.Windows;
using System.Net;
using System.Text;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            String myAd;
            String closedAd;
            if (e.NavigationMode == NavigationMode.New && 
                NavigationContext.QueryString.TryGetValue("myAd", out myAd))
            {
                if (myAd.Equals("true"))
                {
                    this.ApplicationBar.Buttons.RemoveAt(2);
                    this.ApplicationBar.Buttons.RemoveAt(1);
                    this.ApplicationBar.Buttons.RemoveAt(0);

                    ApplicationBarIconButton editAdButton = new ApplicationBarIconButton(new Uri("/Icons/appbar.edit.rest.png", UriKind.Relative));
                    editAdButton.Click += EditAppBarButton_Click;
                    editAdButton.Text = "uredi";
                    this.ApplicationBar.Buttons.Add(editAdButton);

                    ApplicationBarIconButton closeAdButton = new ApplicationBarIconButton(new Uri("/Icons/appbar.delete.rest.png", UriKind.Relative));
                    closeAdButton.Click += CloseAppBarButton_Click;
                    closeAdButton.Text = "zatvori";
                    this.ApplicationBar.Buttons.Add(closeAdButton);
                }
            }
            else if (NavigationContext.QueryString.TryGetValue("closedAd", out closedAd))
            {
                if (closedAd.Equals("true"))
                {
                    this.ApplicationBar.Buttons.RemoveAt(2);
                    this.ApplicationBar.Buttons.RemoveAt(1);
                    this.ApplicationBar.Buttons.RemoveAt(0);                    
                }
            }
            else
            {
                
            }
        }

        private void CallAppBarButton_Click(object sender, System.EventArgs e)
        {            
            _viewModel.CallPhoneCommand.Execute(null);
        }

        private void EditAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NewAdPage.xaml?editAd=true", UriKind.Relative));
        }

        private void CloseAppBarButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(ErrorStrings.CloseAdWarnMessage, "Zatvori oglas", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
            {
                _viewModel.CloseAd();
            }
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