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
            this.DataContext = App.MyAdsViewModel = new MyAdsViewModel();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.ViewModel.IsDataLoading)
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
            if (((FavoritesScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= FavoritesScrollViewer.ScrollableHeight - FavoritesScrollViewer.ScrollableHeight / 5)
                 || (FavoritesScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= FavoritesScrollViewer.ScrollableHeight - FavoritesScrollViewer.ScrollableHeight / 5)))
            {
                App.MyAdsViewModel.LoadFavoriteAdsAsync();
            }
        }

        private void favorites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as AdViewModel;
            if (item == null)
                return;

            App.SelectedAd = item;
            NavigationService.Navigate(new Uri("/Views/DetailsPage.xaml", UriKind.Relative));
            (sender as ListBox).SelectedIndex = -1;
        }

        private void ActiveScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (((ActiveScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= ActiveScrollViewer.ScrollableHeight - ActiveScrollViewer.ScrollableHeight / 5)
                 || (ActiveScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= ActiveScrollViewer.ScrollableHeight - ActiveScrollViewer.ScrollableHeight / 5)))
            {
                App.MyAdsViewModel.LoadActiveAdsAsync();
            }
        }

        private void active_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as AdViewModel;
            if (item == null)
                return;

            App.SelectedAd = item;
            NavigationService.Navigate(new Uri("/Views/DetailsPage.xaml?myAd=true", UriKind.Relative));
            (sender as ListBox).SelectedIndex = -1;
        }

        private void ClosedScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (((ClosedScrollViewer.VerticalOffset - e.TotalManipulation.Translation.Y >= ClosedScrollViewer.ScrollableHeight - ClosedScrollViewer.ScrollableHeight / 5)
                 || (ClosedScrollViewer.VerticalOffset - e.FinalVelocities.LinearVelocity.Y >= ClosedScrollViewer.ScrollableHeight - ClosedScrollViewer.ScrollableHeight / 5)))
            {
                App.MyAdsViewModel.LoadClosedAdsAsync();
            }
        }

        private void closed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as AdViewModel;
            if (item == null)
                return;

            App.SelectedAd = item;
            NavigationService.Navigate(new Uri("/Views/DetailsPage.xaml?closedAd=true", UriKind.Relative));
            (sender as ListBox).SelectedIndex = -1;
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Panorama.SelectedIndex == 0)
            {
                if (!App.MyAdsViewModel.IsFavoriteAdsLoaded)
                {
                    App.MyAdsViewModel.LoadFavoriteAdsAsync();
                }
            }
            else if (Panorama.SelectedIndex == 1)
            {
                if (!App.MyAdsViewModel.IsActiveAdsLoaded)
                {
                    App.MyAdsViewModel.LoadActiveAdsAsync();
                }
            }
            else if (Panorama.SelectedIndex == 2)
            {
                if (!App.MyAdsViewModel.IsClosedAdsLoaded)
                {
                    App.MyAdsViewModel.LoadClosedAdsAsync();
                }
            }
        }
    }
}