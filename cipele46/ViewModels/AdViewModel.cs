using cipele46.Model;
using Microsoft.Phone.Tasks;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace cipele46.ViewModels
{
    public class AdViewModel : ViewModelBaseEx
    {
        private ad _model;
        private BitmapImage _image;

        public string Title { get { return _model.title; } }
        public string Description { get { return _model.description; } }

        public string ImageUrl
        {
            get
            {
                if (_model.image != null &&
                    _model.image.medium != null)
                    return _model.image.medium.url;

                return null;
            }
        }

        public string LargeImageUrl
        {
            get
            {
                if (_model.image != null &&
                    _model.image.mob_large != null)
                    return _model.image.mob_large.url;

                return null;
            }
        }

        public BitmapImage Image
        {
            get
            {
                if (_image != null)
                    return _image;

                ImageDownloader.GetImageAsync(ImageUrl).ContinueWith(t =>
                    {
                        _image = t.Result;
                        RaisePropertyChanged();
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                // TODO: return some default image, vNext
                return ImageDownloader.DefaultImage;
            }
        }

        public BitmapImage LargeImage
        {
            get
            {
                if (_image != null)
                    return _image;

                ImageDownloader.GetImageAsync(LargeImageUrl).ContinueWith(t =>
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
                return (types)_model.ad_type;
            }
        }

        public string Category
        {
            get
            {
                category category;
                if (App.Categories != null)
                {
                    category = App.Categories.FirstOrDefault(i => i.id == _model.category_id);
                }
                else
                {
                    category = App.GetCategoriesAsync().Result.FirstOrDefault(i => i.id == _model.category_id);
                }
                if (category != null)
                    return category.name;

                return ErrorStrings.UnknownCategory;
            }
        }

        public string Location
        {
            get
            {
                var counties = App.Counties;
                if (counties == null)
                {
                    counties = App.GetCountiesAsync().Result;
                }
                
                var city = counties.Where(i => i.cities != null)
                                   .SelectMany(i => i.cities)
                                   .FirstOrDefault(i => i.id == _model.city_id);
                var county = counties.FirstOrDefault(i => i.cities != null &&
                                                        
                    i.cities.Contains(city));
                if (county == null && city == null)
                    return ErrorStrings.UnknownCityAndCounty;

                if (county == null)
                    return city.name;
                if (city == null)
                    return string.Format(ErrorStrings.UnknownCity, county);

                return string.Format("{0}, {1}", county.name, city.name);
            }
        }

        public bool HasWarningMessage
        {
            get { return _model.status != (int)status.active; }
        }

        public string WarningMessage
        {
            get
            {
                switch (_model.status)
                {
                    case (int)status.closed:
                        return Strings.AdIsClosed;
                    case (int)status.pending:
                        return Strings.AdIsPendingAdministratorApproval;
                }

                return string.Empty;
            }
        }

        public bool IsSupply { get { return _model.ad_type == (int)types.supply; } }
        public bool IsDemand { get { return _model.ad_type == (int)types.demand; } }

        public string Expires
        {
            get { return "21 dan"; }
        }

        public string Phone { get { return _model.phone; } }
        public string Mail { get { return _model.email; } }
        public string Id { get { return _model.id.ToString(); } }

        public bool HasPhone { get { return !string.IsNullOrWhiteSpace(_model.phone); } }
        public bool HasMail { get { return !string.IsNullOrWhiteSpace(_model.email); } }

        public ICommand CallPhoneCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }

        public AdViewModel(ad ad)
        {
            _model = ad;

            CallPhoneCommand = new GalaSoft.MvvmLight.Command.RelayCommand(CallPhone);
            SendMessageCommand = new GalaSoft.MvvmLight.Command.RelayCommand(SendMessage);
        }

        void CallPhone()
        {
            var task = new PhoneCallTask()
            {
                PhoneNumber = _model.phone
            };
            task.Show();
        }

        void SendMessage()
        {
        }

        
    }
}
