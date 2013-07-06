using Microsoft.Phone.Controls;

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
    }
}