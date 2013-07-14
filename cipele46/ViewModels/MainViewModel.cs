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
        private const int pageSize = 5;
        private int _allAdsPage = 1;
        private int _demandAdsPage = 1;
        private int _supplyAdsPage = 1;

        private bool _hasMoreAllAds = true;
        private bool _hasMoreDemandAds = true;
        private bool _hasMoreSupplyAds = true;

        private bool _isAllAdsLoading;
        private bool _isDemandAdsLoading;
        private bool _isSupplyAdsLoading;

        private bool _isAllAdsLoaded;
        private bool _isDemandAdsLoaded;
        private bool _isSupplyAdsLoaded;

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
        public bool IsAllAdsLoaded
        {
            get { return _isAllAdsLoaded; }
            set { Set(ref _isAllAdsLoaded, value); }
        }
        public bool IsDemandAdsLoaded
        {
            get { return _isDemandAdsLoaded; }
            set { Set(ref _isDemandAdsLoaded, value); }
        }
        public bool IsSupplyAdsLoaded
        {
            get { return _isSupplyAdsLoaded; }
            set { Set(ref _isSupplyAdsLoaded, value); }
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

        public void ClearData()
        {
            Ads.Clear();
            SupplyAds.Clear();
            DemandAds.Clear();
            _allAdsPage = 1;
            _demandAdsPage = 1;
            _supplyAdsPage = 1;
            _isAllAdsLoaded = false;
            _isSupplyAdsLoaded = false;
            _isDemandAdsLoaded = false;
            _hasMoreAllAds = true;
            _hasMoreDemandAds = true;
            _hasMoreSupplyAds = true;
            IsDataLoaded = false;
        }

        internal async Task LoadAllAdsAsync()
        {
            if (_isAllAdsLoading || !_hasMoreAllAds)
                return;
            _isAllAdsLoading = true;            
            IsDataLoading = true;
            
            var taskAds = new WebClient().DownloadStringTaskAsync(getAdsUrl(_allAdsPage));
            await TaskEx.WhenAll(App.GetCategoriesAsync(),
                                 App.GetCountiesAsync(), 
                                 taskAds);

            var data = await taskAds;
            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                Ads.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreAllAds = false;
            }
            ++_allAdsPage;

            _isAllAdsLoading = false;
            _isAllAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        internal async Task LoadDemandAdsAsync()
        {
            if (_isDemandAdsLoading || !_hasMoreDemandAds)
                return;
            _isDemandAdsLoading = true;
            IsDataLoading = true;

            var taskAds = new WebClient().DownloadStringTaskAsync(getAdsUrl(_demandAdsPage) + "&ad_type=2");
            await TaskEx.WhenAll(App.GetCategoriesAsync(),
                                 App.GetCountiesAsync(),
                                 taskAds);

            var data = await taskAds;
            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                DemandAds.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreDemandAds = false;
            }
            ++_demandAdsPage;

            _isDemandAdsLoading = false;
            _isDemandAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        internal async Task LoadSupplyAdsAsync()
        {
            if (_isSupplyAdsLoading || !_hasMoreSupplyAds)
                return;
            _isSupplyAdsLoading = true;
            IsDataLoading = true;

            var taskAds = new WebClient().DownloadStringTaskAsync(getAdsUrl(_supplyAdsPage) + "&ad_type=1");
            await TaskEx.WhenAll(App.GetCategoriesAsync(),
                                 App.GetCountiesAsync(),
                                 taskAds);

            var data = await taskAds;
            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                SupplyAds.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreSupplyAds = false;
            }
            ++_supplyAdsPage;

            _isSupplyAdsLoading = false;
            _isSupplyAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        private String getAdsUrl(int page)
        {
            String adsUrl = Endpoints.AdsUrl + "?page=" + page + "&per_page=" + pageSize + "&";
            if (((App)Application.Current).CategoryFilter.id != 0)
            {
                adsUrl += "category_id=" + ((App)Application.Current).CategoryFilter.id + "&";
            }
            if (((App)Application.Current).CountyFilter.id != 0)
            {
                adsUrl += "region_id=" + ((App)Application.Current).CountyFilter.id;
            }
            adsUrl += "nocache=" + DateTime.Now.Ticks.ToString();
            return adsUrl;
        }
    }
}
