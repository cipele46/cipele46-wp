using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;

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
            NavigationService.Navigate(new Uri("/Views/SendMessagePage.xaml", UriKind.Relative));
        }

        private void FavoriteAppBarButton_Click(object sender, System.EventArgs e)
        {

        }
    }
}