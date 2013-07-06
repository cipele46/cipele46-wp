using Newtonsoft.Json;
using System.Threading.Tasks;

namespace cipele46
{
    /// <summary>
    /// Exposes asynchronous versions of methods in JsonConvert.
    /// </summary>
    public static class JsonConvertEx
    {
        public static Task<T> DeserializeObjectAsync<T>(string value)
        {
            return TaskEx.Run(() => JsonConvert.DeserializeObject<T>(value));
        }
    }
}
