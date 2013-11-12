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

        public ObservableCollection<category> Categories { get; set; }
        public ObservableCollection<county> Counties { get; set; }

        public NewAdViewModel()
        {            
            Categories = new ObservableCollection<category>();
            Counties = new ObservableCollection<county>();
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

        public async void PostAnAd(cipele46.Views.NewAdPage.postAnAd postAnAd)
        {
            IsDataLoading = true;
            IsDataLoaded = false;
            var request = WebRequest.CreateHttp(Endpoints.PostAnAdUrl);
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

            IsDataLoading = false;
            IsDataLoaded = true;

            var response = await request.GetResponseAsync() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                    ((App)Application.Current).RootFrame.Navigate(new System.Uri("/Views/PostAdSuccessPage.xaml", System.UriKind.Relative));
                //});
            }

        }
    }
}
