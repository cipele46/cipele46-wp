using cipele46.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cipele46.ViewModels
{
    public class MyAdsViewModel : ViewModelBaseEx
    {
        private const int pageSize = 15;
        private int _favoriteAdsPage = 1;
        private int _activeAdsPage = 1;
        private int _closedAdsPage = 1;

        private bool _hasMoreFavoriteAds = true;
        private bool _hasMoreActiveAds = true;
        private bool _hasMoreClosedAds = true;

        private bool _isFavoriteAdsLoading;
        private bool _isActiveAdsLoading;
        private bool _isClosedAdsLoading;

        private bool _isFavoriteAdsLoaded;
        private bool _isActiveAdsLoaded;
        private bool _isClosedAdsLoaded;

        private bool _isDataLoading;
        private bool _isDataLoaded;

        private ObservableCollection<AdViewModel> _favoriteAds;
        private ObservableCollection<AdViewModel> _activeAds;
        private ObservableCollection<AdViewModel> _closedAds;

        public MyAdsViewModel()
        {
            _favoriteAds = new ObservableCollection<AdViewModel>();
            _activeAds = new ObservableCollection<AdViewModel>();
            _closedAds = new ObservableCollection<AdViewModel>();
        }

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
        public bool IsFavoriteAdsLoaded
        {
            get { return _isFavoriteAdsLoaded; }
            set { Set(ref _isFavoriteAdsLoaded, value); }
        }
        public bool IsActiveAdsLoaded
        {
            get { return _isActiveAdsLoaded; }
            set { Set(ref _isActiveAdsLoaded, value); }
        }
        public bool IsClosedAdsLoaded
        {
            get { return _isClosedAdsLoaded; }
            set { Set(ref _isClosedAdsLoaded, value); }
        } 

        public ObservableCollection<AdViewModel> FavoriteAds
        {
            get { return _favoriteAds; }
            set { Set(ref _favoriteAds, value); }
        }

        public ObservableCollection<AdViewModel> ActiveAds
        {
            get { return _activeAds; }
            set { Set(ref _activeAds, value); }
        }

        public ObservableCollection<AdViewModel> ClosedAds
        {
            get { return _closedAds; }
            set { Set(ref _closedAds, value); }
        }

        internal async Task LoadFavoriteAdsAsync()
        {
            if (_isFavoriteAdsLoading || !_hasMoreFavoriteAds)
                return;
            _isFavoriteAdsLoading = true;
            IsDataLoading = true;

            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ((App)Application.Current).User.email, ((App)Application.Current).User.password)));
            var data = await client.DownloadStringTaskAsync(getMyAdsUrl(_favoriteAdsPage) + "&favorites=1");

            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                FavoriteAds.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreFavoriteAds = false;
            }
            ++_favoriteAdsPage;

            _isFavoriteAdsLoading = false;
            _isFavoriteAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        internal async Task LoadActiveAdsAsync()
        {
            if (_isActiveAdsLoading || !_hasMoreActiveAds)
                return;
            _isActiveAdsLoading = true;
            IsDataLoading = true;

            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ((App)Application.Current).User.email, ((App)Application.Current).User.password)));
            var data = await client.DownloadStringTaskAsync(getMyAdsUrl(_activeAdsPage) + "&user=1&status=2");

            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                ActiveAds.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreActiveAds = false;
            }
            ++_activeAdsPage;

            _isActiveAdsLoading = false;
            _isActiveAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        internal async Task LoadClosedAdsAsync()
        {
            if (_isClosedAdsLoading || !_hasMoreClosedAds)
                return;
            _isClosedAdsLoading = true;
            IsDataLoading = true;

            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ((App)Application.Current).User.email, ((App)Application.Current).User.password)));
            var data = await client.DownloadStringTaskAsync(getMyAdsUrl(_closedAdsPage) + "&user=1&status=3");

            int resultsNumber = 0;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                ++resultsNumber;
                ClosedAds.Add(new AdViewModel(ad));
            }

            if (resultsNumber < pageSize)
            {
                _hasMoreClosedAds = false;
            }
            ++_closedAdsPage;

            _isClosedAdsLoading = false;
            _isClosedAdsLoaded = true;
            IsDataLoading = false;
            IsDataLoaded = true;
        }

        private String getMyAdsUrl(int page)
        {
            String adsUrl = Endpoints.AdsUrl + "?page=" + page + "&per_page=" + pageSize + "&";            
            adsUrl += "nocache=" + DateTime.Now.Ticks.ToString();
            return adsUrl;
        }
    }
}
