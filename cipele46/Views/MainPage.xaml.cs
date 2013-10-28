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
using System.Windows.Navigation;

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
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }

            if (!App.ViewModel.IsDataLoaded)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    App.ViewModel.LoadAllAdsAsync();
                }
                else
                {
                    if (Panorama.SelectedIndex == 0)
                    {
                        App.ViewModel.LoadAllAdsAsync();
                    }
                    else if (Panorama.SelectedIndex == 1)
                    {
                        App.ViewModel.LoadSupplyAdsAsync();
                    }
                    else if (Panorama.SelectedIndex == 2)
                    {
                        App.ViewModel.LoadDemandAdsAsync();
                    }
                }
            }

            String filterText = ((App)Application.Current).CategoryFilter.name + "\n" + ((App)Application.Current).CountyFilter.name;
            FilterTextBlock.Text = FilterTextBlockPonuda.Text = FilterTextBlockPotraznja.Text = filterText;

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
                var request = HttpWebRequest.CreateHttp(Endpoints.LoginUserUrl); //"http://cipele46.org/users/show.json");
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

        private void FilterTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FilterAppBarButton_Click(sender, e);
        }

        private void AllScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (((AllScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= AllScrollViewer.ScrollableHeight - AllScrollViewer.ScrollableHeight / 5)
                 || (AllScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= AllScrollViewer.ScrollableHeight - AllScrollViewer.ScrollableHeight / 5)))
            {
                App.ViewModel.LoadAllAdsAsync();
            }
        }

        private void SupplyScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (((SupplyScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= SupplyScrollViewer.ScrollableHeight - SupplyScrollViewer.ScrollableHeight / 5)
                 || (SupplyScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= SupplyScrollViewer.ScrollableHeight - SupplyScrollViewer.ScrollableHeight / 5)))
            {
                App.ViewModel.LoadSupplyAdsAsync();
            }
        }

        private void DemandScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (((DemandScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= DemandScrollViewer.ScrollableHeight - DemandScrollViewer.ScrollableHeight / 5)
                 || (DemandScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= DemandScrollViewer.ScrollableHeight - DemandScrollViewer.ScrollableHeight / 5)))
            {
                App.ViewModel.LoadDemandAdsAsync();
            }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Panorama.SelectedIndex == 0)
            {
                if (!App.ViewModel.IsAllAdsLoaded)
                {
                    App.ViewModel.LoadAllAdsAsync();
                }
            }
            else if (Panorama.SelectedIndex == 1)
            {
                if (!App.ViewModel.IsSupplyAdsLoaded)
                {
                    App.ViewModel.LoadSupplyAdsAsync();
                }
            }
            else if (Panorama.SelectedIndex == 2)
            {
                if (!App.ViewModel.IsDemandAdsLoaded)
                {
                    App.ViewModel.LoadDemandAdsAsync();
                }
            }
        }
        
    }
}