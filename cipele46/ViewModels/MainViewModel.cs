using System.Net;

namespace cipele46.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        public bool IsDataLoaded { get; set; }

        internal async void LoadDataAsync()
        {
            var client = new WebClient();
            var data = await client.DownloadStringTaskAsync("http://dev.fiveminutes.eu/cipele/api/categories");
            var categories = await JsonConvertEx.DeserializeObjectAsync<Model.categories>(data);
        }
    }
}
