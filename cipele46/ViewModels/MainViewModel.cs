using System.Net;

namespace cipele46.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        public bool IsDataLoading { get; set; }
        public bool IsDataLoaded { get; set; }

        internal async void LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            var client = new WebClient();
            var data = await client.DownloadStringTaskAsync("http://dev.fiveminutes.eu/cipele/api/categories");
            var categories = await JsonConvertEx.DeserializeObjectAsync<Model.category[]>(data);

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
