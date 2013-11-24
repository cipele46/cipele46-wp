using cipele46.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace cipele46.ViewModels
{
    public class NewAdViewModel : ViewModelBaseEx
    {
        private String _title;
        private String _description;
        private String _email;
        private String _phone;
        private int _countyId = 0;
        private int _categoryId = 0;
        private int _cityId = 0;
        private int _adType = 1;

        public ObservableCollection<category> Categories { get; set; }
        public ObservableCollection<county> Counties { get; set; }
        public ObservableCollection<city> Cities { get; set; }

        public NewAdViewModel()
        {            
            Categories = new ObservableCollection<category>();
            Counties = new ObservableCollection<county>();
            Cities = new ObservableCollection<city>();
        }

        public ad Ad
        {
            set
            {
                Title = value.title;
                Description = value.description;
                Email = value.email;
                Phone = value.phone;
                CountyId = value.region_id;
                CategoryId = value.category_id;
                CityId = value.city_id;
                AdType = value.ad_type;
            }
        }

        public String Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public String Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        public String Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public String Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        public int CountyId
        {
            get { return _countyId; }
            set { Set(ref _countyId, value); }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { Set(ref _categoryId, value); }
        }

        public int CityId
        {
            get { return _cityId; }
            set { Set(ref _cityId, value); }                
        }

        public int AdType
        {
            get { return _adType; }
            set { Set(ref _adType, value); }
        }

        public async void PostAnAd(cipele46.Views.NewAdPage.postAnAd postAnAd, bool isEdit)
        {
            IsDataLoading = true;
            IsDataLoaded = false;
            String url = Endpoints.PostAnAdUrl;
            if (isEdit)
                url += "/" + App.SelectedAd.Ad.id;
            var request = WebRequest.CreateHttp(url);
            if (isEdit)
                request.Method = "PUT";
            else
                request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ((App)Application.Current).User.email, ((App)Application.Current).User.password)));

            var data = JsonConvert.SerializeObject(postAnAd);

            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Prepare web request...
            var newStream = await request.GetRequestStreamAsync();
            // Send the data.
            newStream.Write(byteData, 0, byteData.Length);
            newStream.Close();

            var response = await request.GetResponseAsync() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                if (isEdit)
                    ((App)Application.Current).RootFrame.Navigate(new System.Uri("/Views/PostAdSuccessPage.xaml?editAd=true", System.UriKind.Relative));
                else
                    ((App)Application.Current).RootFrame.Navigate(new System.Uri("/Views/PostAdSuccessPage.xaml", System.UriKind.Relative));
                //});
            }

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
