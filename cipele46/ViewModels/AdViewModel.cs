using cipele46.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace cipele46.ViewModels
{
    public class AdViewModel : ViewModelBaseEx
    {
        private ad _model;
        private BitmapImage _image;

        public string Title { get { return _model.title; } }
        public string Description { get { return _model.description; } }
        public string ImageUrl { get { return _model.imageUrl; } }

        public BitmapImage Image
        {
            get
            {
                if (_image != null)
                    return _image;

                ImageDownloader.GetImageAsync(_model.imageUrl).ContinueWith(t =>
                    {
                        _image = t.Result;
                        RaisePropertyChanged();
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                // TODO: return some default image, vNext
                return ImageDownloader.DefaultImage;
            }
        }

        public types Type
        {
            get
            {
                return (types)_model.type;
            }
        }

        public string Category
        {
            get
            {
                var category = App.GetCategoriesAsync().Result.FirstOrDefault(i => i.id == _model.categoryID);
                if (category != null)
                    return category.name;

                return ErrorStrings.UnknownCategory;
            }
        }

        public string Location
        {
            get
            {
                var county = App.GetCountiesAsync().Result.FirstOrDefault(i => i.id == _model.districtID);
                var city = App.GetCountiesAsync().Result.SelectMany(i => i.cities)
                    .FirstOrDefault(i => i.id == _model.cityID);
                
                if (county == null && city == null)
                    return ErrorStrings.UnknownCityAndCounty;

                if (county == null)
                    return city.name;
                if (city == null)
                    return string.Format(ErrorStrings.UnknownCity, county);

                return string.Format("{0}, {1}", county.name, city.name);
            }
        }

        public AdViewModel(ad ad)
        {
            _model = ad;
        }
    }
}
