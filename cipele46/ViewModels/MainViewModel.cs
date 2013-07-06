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
        private ObservableCollection<category> _categories;
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

        public ObservableCollection<category> Categories
        {
            get { return _categories; }
            set { Set(ref _categories, value); }
        }

        public MainViewModel()
        {
            _categories = new ObservableCollection<category>();
        }

        internal async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            var client = new WebClient();
            var data = await client.DownloadStringTaskAsync("http://dev.fiveminutes.eu/cipele/api/categories");
            foreach (var category in await JsonConvertEx.DeserializeObjectAsync<Model.category[]>(data))
                Categories.Add(category);

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
