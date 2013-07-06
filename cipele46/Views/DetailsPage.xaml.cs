using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using cipele46.Model;
using System.Windows;

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
            user user = ((App)Application.Current).User;
            if (user == null)
            {
                MessageBoxResult result = MessageBox.Show("Za slanje poruke potrebna je prijava", "Prijavite se", MessageBoxButton.OKCancel);
                if (result.Equals(MessageBoxResult.OK))
                {
                    NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
                }               
            }
            else
            {
                NavigationService.Navigate(new Uri("/Views/SendMessagePage.xaml", UriKind.Relative));
            }
        }

        private void FavoriteAppBarButton_Click(object sender, System.EventArgs e)
        {

        }
    }
}