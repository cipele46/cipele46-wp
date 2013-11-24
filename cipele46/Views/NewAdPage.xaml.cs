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
        private bool isEdit = false;

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

            string editAd;
            if (e.NavigationMode == NavigationMode.New
                && NavigationContext.QueryString.TryGetValue("editAd", out editAd))
            {
                if (editAd == "true")
                {
                    App.NewAdViewModel.Ad = App.SelectedAd.Ad;
                    PageTitle.Text = "uredi oglas";
                    isEdit = true;
                }
            }

            AdTypePicker.SelectedIndex = App.NewAdViewModel.AdType - 1;

            if (App.Categories == null)
                App.GetCategoriesAsync();

            if (App.Counties == null)
                App.GetCountiesAsync();

            App.NewAdViewModel.Categories = new ObservableCollection<category>(App.Categories);
            if (App.NewAdViewModel.Categories != null)
            {
                CategoriesPicker.ItemsSource = App.NewAdViewModel.Categories;
                category category = null;
                try
                {
                    category = App.Categories.Single(c => c.id == App.NewAdViewModel.CategoryId);
                }
                catch (Exception ex)
                {
                }
                if (category != null)
                    CategoriesPicker.SelectedIndex = CategoriesPicker.Items.IndexOf(category);
                else
                    CategoriesPicker.SelectedItem = CategoriesPicker.Items[0];
            }

            var counties = App.Counties;
            if (counties == null)
                counties = App.GetCountiesAsync().Result;

            var city = counties.Where(i => i.cities != null)
                               .SelectMany(i => i.cities)
                               .FirstOrDefault(i => i.id == App.SelectedAd.Ad.city_id);

            var county = counties.FirstOrDefault(i => i.cities != null 
                && i.cities.Contains(city));

            App.NewAdViewModel.Counties = new ObservableCollection<county>(counties);
            CountyPicker.ItemsSource = App.NewAdViewModel.Counties;
            if (county != null)
                CountyPicker.SelectedIndex = CountyPicker.Items.IndexOf(county);
            else
                CountyPicker.SelectedItem = CountyPicker.Items[0];

            if (App.Counties != null)
            {
                App.NewAdViewModel.Cities = new ObservableCollection<city>(county.cities);
                if (App.NewAdViewModel.Cities != null)
                {
                    CityPicker.ItemsSource = App.NewAdViewModel.Cities;
                    if (city != null)
                        CityPicker.SelectedIndex = CityPicker.Items.IndexOf(city);
                    else
                        CityPicker.SelectedItem = CityPicker.Items[0];
                }
            }
        }

        private void NewAdAppBarButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TitleTextBox.Text)
                || String.IsNullOrWhiteSpace(EmailTextBox.Text)
                || String.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                || AdTypePicker.SelectedItem == null
                || CategoriesPicker.SelectedItem == null
                || CityPicker.SelectedItem == null
                || CountyPicker.SelectedItem == null)
            {
                MessageBox.Show(ErrorStrings.NewAdDataMissing);
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
            App.NewAdViewModel.PostAnAd(postAnAd, isEdit);
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