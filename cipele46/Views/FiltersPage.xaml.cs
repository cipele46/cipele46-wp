using cipele46.ViewModels;
using Microsoft.Phone.Controls;
using System;

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

        }
    }
}