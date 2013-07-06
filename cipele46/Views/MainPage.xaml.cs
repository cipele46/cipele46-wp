using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Coding4Fun.Toolkit.Controls;
using cipele46.ViewModels;
using System.Text;

namespace cipele46
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool backKeyPressed = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += MainPage_Loaded;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadDataAsync();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            String filterText = "Kategorija: " + ((App)Application.Current).CategoryFilter.name + "\nŽupanija: " + ((App)Application.Current).CountyFilter.name;
            FilterTextBlock.Text = filterText;

            lbAll.SelectedItem = null;
            lbSupply.SelectedItem = null;
            lbDemand.SelectedItem = null;
        }

        private void FilterAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/FiltersPage.xaml", UriKind.Relative));
        }

        private void SearchAppBarButton_Click(object sender, EventArgs e)
        {
            InputPrompt inputPrompt = new InputPrompt
            {
                Title = "traži",
                Message = ""
            };

            inputPrompt.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(inputPrompt_Completed);
            backKeyPressed = false;
            inputPrompt.Show();
        }

        void inputPrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (backKeyPressed || String.IsNullOrWhiteSpace(((InputPrompt)sender).Value))
            {
                return;
            }
            String searchKeyword = ((InputPrompt)sender).Value;
            NavigationService.Navigate(new Uri(string.Format("/Views/SearchPage.xaml?searchKeyword={0}", searchKeyword), UriKind.Relative));
        }

        private async void MyAdsAppBarButton_Click(object sender, EventArgs e)
        {
            Uri myAdsPageUri = new Uri("/Views/MyAdsPage.xaml", UriKind.Relative);
            if (((App)Application.Current).User == null)
            {
                NavigationService.Navigate(new Uri(String.Format("/Views/LoginPage.xaml?successUri={0}", myAdsPageUri.OriginalString), UriKind.Relative));
            }
            else
            {
                var user = ((App)Application.Current).User;
                // try logging in immediately
                var request = HttpWebRequest.CreateHttp("http://cipele46.org/users/show.json");
                request.Method = "GET";
                request.Accept = "application/json";
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
                var response = await request.GetResponseAsync() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                    NavigationService.Navigate(myAdsPageUri);
                else
                {
                    MessageBox.Show("Neispravan mail ili lozinka");
                    NavigationService.Navigate(new Uri(String.Format("/Views/LoginPage.xaml?successUri={0}", myAdsPageUri.OriginalString), UriKind.Relative));
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as AdViewModel;
            if (item == null)
                return;

            App.SelectedAd = item;
            NavigationService.Navigate(new Uri("/Views/DetailsPage.xaml", UriKind.Relative));
        }

        private void FilterTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FilterAppBarButton_Click(sender, e);
        }
    }
}