using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook;
using cipele46.Model;

namespace cipele46.Views
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string appId = "187448291422507";
        private const string appSecret = "1cc29696449f87708eae7223dbe074e4";
        private string userId;
        private string email;
        private string fullName;
        private string accessToken;
        private const string ExtendedPermissions = "email";

        private readonly FacebookClient _fb = new FacebookClient();

        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        private void webBrowser1_Navigated(object sender, NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["client_secret"] = appSecret;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);            
        }

        private void LoginSucceded(string accessToken)
        {
            //loadingControl.Visibility = Visibility.Visible;
            //webBrowser1.Visibility = Visibility.Collapsed;

            // logout from facebook web control
            var parameters = new Dictionary<string, object>();
            parameters["access_token"] = accessToken;
            parameters["next"] = "https://www.facebook.com/connect/login_success.html";
            var logoutUrl = _fb.GetLogoutUrl(parameters);
            var webBrowserLogout = new WebBrowser();
            webBrowserLogout.Navigated += (o, args) =>
            {
            };
            webBrowserLogout.Navigate(logoutUrl);

            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                userId = (string)result["id"];
                email = (string)result["email"];
                fullName = (string)result["name"];

                loginToServer();
            };

            fb.GetAsync("me");
        }

        private void loginToServer()
        {
            ((App)Application.Current).User = new user();
            Dispatcher.BeginInvoke(() =>
            {                
                NavigationService.Navigate(new Uri("/Views/MyAdsPage.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            });
            
        }
    }
}