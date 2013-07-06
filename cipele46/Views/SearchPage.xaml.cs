using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace cipele46.Views
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PhoneApplicationPage_Loaded);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            String searchKeyword;
            NavigationContext.QueryString.TryGetValue("searchKeyword", out searchKeyword);
            MessageBox.Show(searchKeyword);
        }
    }
}