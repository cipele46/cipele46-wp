using cipele46.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cipele46.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private ObservableCollection<AdViewModel> _ads;
        private bool _isDataLoading;
        private bool _isDataLoaded;

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

        public MainViewModel()
        {
            _ads = new ObservableCollection<AdViewModel>();
        }

        internal async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            var data = await new WebClient().DownloadStringTaskAsync(Endpoints.AdsSampleUrl);
            foreach (var ad in await JsonConvertEx.DeserializeObjectAsync<ad[]>(data))
                Ads.Add(new AdViewModel(ad));

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
