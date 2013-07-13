using cipele46.ViewModels;
using cipele46.Model;
using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace cipele46.Views
{
    public partial class FiltersPage : PhoneApplicationPage
    {
        FilterViewModel _viewModel;

        public FiltersPage()
        {
            InitializeComponent();
            DataContext = _viewModel = new FilterViewModel();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!_viewModel.IsDataLoaded)
                _viewModel.LoadDataAsync();
        }

        private void FiltersAppBarButton_Click(object sender, EventArgs e)
        {
            category categoryFilter = (category)CategoriesPicker.SelectedItem;
            if (categoryFilter.id != ((App)Application.Current).CategoryFilter.id)
            {
                ((App)Application.Current).CategoryFilter = categoryFilter;
                App.ViewModel.IsDataLoaded = false;
            }

            county countyFilter = (county)CountyPicker.SelectedItem;
            if (countyFilter.id != ((App)Application.Current).CountyFilter.id)
            {
                ((App)Application.Current).CountyFilter = countyFilter;
                App.ViewModel.IsDataLoaded = false;
            }

            NavigationService.GoBack();
        }
    }
}