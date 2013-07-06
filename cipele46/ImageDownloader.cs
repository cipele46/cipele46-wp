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

        public static Task<BitmapImage> GetImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return TaskEx.FromResult<BitmapImage>((BitmapImage)null);

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
            var wc = new WebClient();
            var stream = await wc.OpenReadTaskAsync(imageUrl);
            var bi = new BitmapImage();
            bi.SetSource(stream);
            return bi;
        }
    }
}
