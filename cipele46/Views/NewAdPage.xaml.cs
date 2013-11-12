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
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using cipele46.Model;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace cipele46.Views
{
    public partial class NewAdPage : PhoneApplicationPage
    {        
        public NewAdPage()
        {
            InitializeComponent();
            this.DataContext = App.NewAdViewModel;
            if (((App)Application.Current).User != null)
            {
                App.NewAdViewModel.Email = ((App)Application.Current).User.email;
                App.NewAdViewModel.Phone = ((App)Application.Current).User.phone;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.Categories == null)
                App.GetCategoriesAsync();

            if (App.Counties == null)
                App.GetCountiesAsync();

            App.NewAdViewModel.Categories = new ObservableCollection<category>(App.Categories);
            App.NewAdViewModel.Counties = new ObservableCollection<county>(App.Counties);
        }

        private void NewAdAppBarButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(App.NewAdViewModel.Title))
            {
                return;
            }            

            if (String.IsNullOrWhiteSpace(App.NewAdViewModel.Email))
            {
                return;
            }

            postAnAd postAnAd = new postAnAd
            {
                ad = new ad
                {
                    ad_type = AdTypePicker.SelectedIndex + 1,
                    category_id = (CategoriesPicker.SelectedItem as category).id,
                    city_id = (CityPicker.SelectedItem as city).id,
                    description = DescriptionTextBox.Text,
                    title = TitleTextBox.Text,
                    region_id = (CountyPicker.SelectedItem as county).id,
                    email = EmailTextBox.Text,
                    phone = PhoneTextBox.Text
                }
            };
            App.NewAdViewModel.PostAnAd(postAnAd);
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {                        
            PhotoChooserTask photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            photoChooserTask.Show();
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);             
                AddImage.Source = bmp;
            }
        }

        private void CountyPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            county county = CountyPicker.SelectedItem as county;
            if (county != null)
            {
                CityPicker.ItemsSource = county.cities;
                CityPicker.SelectedIndex = 0;
            }
            
        }

        public class postAnAd
        {
            public ad ad;
        }
    }
}