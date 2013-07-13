using cipele46.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cipele46.ViewModels
{
    public class SearchViewModel : ViewModelBaseEx
    {
        private bool _isDataLoading;
        private bool _isDataLoaded;
        private String _searchKeyword;

        private ObservableCollection<AdViewModel> _searchAds;

        public SearchViewModel()
        {
            _searchAds = new ObservableCollection<AdViewModel>();
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

        public ObservableCollection<AdViewModel> SearchAds
        {
            get { return _searchAds; }
            set { Set(ref _searchAds, value); }
        }

        public String SearchKeyword
        {
            get { return _searchKeyword; }
            set { Set(ref _searchKeyword, value); }
        }

        internal async Task SearchDataAsync(String keyword)
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;
            IsDataLoaded = false;

            SearchKeyword = keyword;
            SearchAds.Clear();

            // ensure that categories and counties are loaded along with ads
            var taskAds = new WebClient().DownloadStringTaskAsync(Endpoints.AdsUrl + "?query=" + keyword);
            await TaskEx.WhenAll(taskAds); 

            // .Result is fine also
            var data = await taskAds;
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
            {
                SearchAds.Add(new AdViewModel(ad));
            }

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
