using cipele46.Model;
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
                    });

                // TODO: return some default image, vNext
                return _image;
            }
        }

        public AdViewModel(ad ad)
        {
            _model = ad;
        }
    }
}
