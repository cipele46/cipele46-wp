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
using cipele46.ViewModels;
using System.Windows.Navigation;

namespace cipele46.Views
{
    public partial class MyAdsPage : PhoneApplicationPage
    {
        public MyAdsPage()
        {
            InitializeComponent();
            this.DataContext = App.MyAdsViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.ViewModel.IsDataLoaded)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    App.MyAdsViewModel.LoadFavoriteAdsAsync();
                }
                else
                {
                    if (Panorama.SelectedIndex == 0)
                    {
                        App.MyAdsViewModel.LoadFavoriteAdsAsync();
                    }
                    else if (Panorama.SelectedIndex == 1)
                    {
                        App.MyAdsViewModel.LoadActiveAdsAsync();
                    }
                    else if (Panorama.SelectedIndex == 2)
                    {
                        App.MyAdsViewModel.LoadClosedAdsAsync();
                    }
                }
            }
        }

        private void AddAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NewAdPage.xaml", UriKind.Relative));
        }

        private void SettingsAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void FavoritesScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void favorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ActiveScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void active_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClosedScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void closed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}