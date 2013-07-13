using cipele46.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace cipele46.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private bool _isDataLoading;
        private bool _isDataLoaded;

        private ObservableCollection<AdViewModel> _ads;
        private ObservableCollection<AdViewModel> _supplyAds;
        private ObservableCollection<AdViewModel> _demandAds;

        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set { Set(ref _isDataLoading, value); }
        }
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { Set(ref _isDataLoaded, value); }
        }        

        public ObservableCollection<AdViewModel> Ads
        {
            get { return _ads; }
            set { Set(ref _ads, value); }
        }

        public ObservableCollection<AdViewModel> SupplyAds
        {
            get { return _supplyAds; }
            set { Set(ref _supplyAds, value); }
        }

        public ObservableCollection<AdViewModel> DemandAds
        {
            get { return _demandAds; }
            set { Set(ref _demandAds, value); }
        }

        public MainViewModel()
        {
            _ads = new ObservableCollection<AdViewModel>();
            _supplyAds = new ObservableCollection<AdViewModel>();
            _demandAds = new ObservableCollection<AdViewModel>();
        }

        internal async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            Ads.Clear();
            SupplyAds.Clear();
            DemandAds.Clear();

            // ensure that categories and counties are loaded along with ads
            String adsUrl = Endpoints.AdsUrl + "?";
            if (((App)Application.Current).CategoryFilter.id != 0)
            {
                adsUrl += "category_id=" + ((App)Application.Current).CategoryFilter.id + "&";
            }
            if (((App)Application.Current).CountyFilter.id != 0)
            {
                adsUrl += "region_id=" + ((App)Application.Current).CountyFilter.id;
            }
            var taskAds = new WebClient().DownloadStringTaskAsync(adsUrl);
            await TaskEx.WhenAll(App.GetCategoriesAsync(),
                                 App.GetCountiesAsync(),
                                 taskAds);

            // .Result is fine also
            var data = await taskAds;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                Ads.Add(new AdViewModel(ad));
            }

            SupplyAds.AddRange(Ads.Where(i => i.Type == types.supply));
            DemandAds.AddRange(Ads.Where(i => i.Type == types.demand));

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
