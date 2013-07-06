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

        private void FilterAppBarButton_Click(object sender, EventArgs e)
        {

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
            NavigationContext.QueryString.Add("searchKeyword", searchKeyword);
            NavigationService.Navigate(new Uri("/Views/SearchPage.xaml"));
        }

        private void MyAdsAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MyAdsPage.xaml", UriKind.Relative));
        }
    }
}