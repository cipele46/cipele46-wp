using cipele46.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cipele46
{
    /// <summary>
    /// Class used for shared logic between components
    /// </summary>
    public class Tools
    {
        public static async Task<HttpStatusCode> LoginUser(user user)
        {
            var request = HttpWebRequest.CreateHttp(Endpoints.LoginUserUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user.email, user.password)));
            try
            {
                var response = await request.GetResponseAsync() as HttpWebResponse;

                using (var reader = new StreamReader(response.GetResponseStream())) {                   
                    var userJson = await reader.ReadToEndAsync();
                    user returnedUser = JsonConvert.DeserializeObject<user>(userJson);
                    returnedUser.password = user.password;
                    ((App)Application.Current).User = returnedUser;
                }

                return response.StatusCode;
            }
            catch (Exception e)
            {
                return HttpStatusCode.Forbidden;
            }
        }

        public class registration_info
        {
            public user user { get; set; }
        }
    }
}
