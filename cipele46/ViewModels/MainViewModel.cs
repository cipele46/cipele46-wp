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

        public bool IsDataLoading { get; set; }
        public bool IsDataLoaded { get; set; }

        public ObservableCollection<category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                Set(ref _categories, value);
            }
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
