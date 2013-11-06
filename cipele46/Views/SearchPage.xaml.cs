using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using cipele46.ViewModels;
using Coding4Fun.Toolkit.Controls;

namespace cipele46.Views
{
    public partial class SearchPage : PhoneApplicationPage
    {
        SearchViewModel _viewModel;

        public SearchPage()
        {
            InitializeComponent();
            this.DataContext = _viewModel = new SearchViewModel();
            this.Loaded += new RoutedEventHandler(PhoneApplicationPage_Loaded);            
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            String searchKeyword;
            NavigationContext.QueryString.TryGetValue("searchKeyword", out searchKeyword);

            _viewModel.SearchDataAsync(searchKeyword);

            //MessageBox.Show(searchKeyword);
        }

        private void SearchAppBarButton_Click(object sender, EventArgs e)
        {
            InputPrompt inputPrompt = new InputPrompt
            {
                Title = "traži oglase",
                Message = ""
            };

            inputPrompt.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(inputPrompt_Completed);
            inputPrompt.Show();
        }

        void inputPrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (String.IsNullOrWhiteSpace(((InputPrompt)sender).Value))
            {
                return;
            }
            String searchKeyword = ((InputPrompt)sender).Value;
            _viewModel.SearchDataAsync(searchKeyword);
        }

        private void lbAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as AdViewModel;
            if (item == null)
                return;

            App.SelectedAd = item;
            NavigationService.Navigate(new Uri("/Views/DetailsPage.xaml", UriKind.Relative));
        }
    }
}