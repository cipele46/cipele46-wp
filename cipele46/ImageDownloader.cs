using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace cipele46
{
    internal static class ImageDownloader
    {
        private static object _syncLock = new object();
        private static Dictionary<string, Task<BitmapImage>> _allImages = new Dictionary<string, Task<BitmapImage>>();
        private static BitmapImage _defaultImage;

        public static Task<BitmapImage> GetImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return TaskEx.FromResult<BitmapImage>(DefaultImage);

            lock (_syncLock)
            {
                Task<BitmapImage> task;
                if (_allImages.TryGetValue(imageUrl, out task))
                    return task;

                task = GetImageAsync_impl(imageUrl);
                _allImages.Add(imageUrl, task);
                return task;
            }
        }

        private static async Task<BitmapImage> GetImageAsync_impl(string imageUrl)
        {
            try
            {
                var wc = new WebClient();
                var stream = await wc.OpenReadTaskAsync(imageUrl);
                var bi = new BitmapImage();
                bi.SetSource(stream);
                return bi;
            }
            catch
            {
                return DefaultImage;
            }
        }

        public static BitmapImage DefaultImage
        {
            get
            {
                return _defaultImage ??
                   (_defaultImage = new BitmapImage(new Uri("/cipele46;component/Images/Missing.jpg", UriKind.Relative)));
            }
        }
    }
}
