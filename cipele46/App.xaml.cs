using cipele46.Model;
using cipele46.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace cipele46
{
    public partial class App : Application
    {
        #region Shared stuff

        private static List<category> _categories;

        public static List<category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        static Task<List<category>> _taskCategories = null;

        public static Task<List<category>> GetCategoriesAsync()
        {
            if (_taskCategories != null)
                return _taskCategories;

            return _taskCategories = TaskEx.Run(async () =>
                {
                    var client = new WebClient();
                    var data = await client.DownloadStringTaskAsync(Endpoints.CategoriesUrl);
                    Categories = (await JsonConvertEx.DeserializeObjectAsync<Model.category[]>(data)).ToList();                    
                    return Categories;
                });
        }

        private static List<county> _counties;

        public static List<county> Counties
        {
            get { return _counties; }
            set { _counties = value; }
        }

        static Task<List<county>> _taskCounties = null;

        public static Task<List<county>> GetCountiesAsync()
        {
            if (_taskCounties != null)
                return _taskCounties;

            return _taskCounties = TaskEx.Run(async () =>
            {
                var client = new WebClient();
                var data = await client.DownloadStringTaskAsync(Endpoints.CountiesUrl);                
                Counties = (await JsonConvertEx.DeserializeObjectAsync<Model.county[]>(data)).ToList();                
                return Counties;
            });
        }

        #endregion

        private static MainViewModel viewModel = null;

        public user User
        {
            get
            {
                IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                if (isoStore.Contains("user"))
                {
                    return (user)isoStore["user"];
                }
                else
                {
                    return null;
                }               
            }
            set
            {
                IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                if (isoStore.Contains("user"))
                {
                    isoStore["user"] = value;
                }
                else
                {
                    isoStore.Add("user", value);
                } 
            }
        }

        public county CountyFilter
        {
            get
            {
                IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                if (isoStore.Contains("CountyFilter"))
                {
                    return (county)isoStore["CountyFilter"];
                }
                else
                {
                    county countyAll = new county();
                    countyAll.name = "Sve županije";
                    countyAll.id = 0;
                    return countyAll;
                }
            }
            set
            {
                if (value != null)
                {
                    IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                    if (isoStore.Contains("CountyFilter"))
                    {
                        isoStore["CountyFilter"] = value;
                    }
                    else
                    {
                        isoStore.Add("CountyFilter", value);
                    }
                }
            }
        }

        public category CategoryFilter
        {
            get
            {
                IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                if (isoStore.Contains("CategoryFilter"))
                {
                    return (category)isoStore["CategoryFilter"];
                }
                else
                {
                    category categoryAll = new category();
                    categoryAll.name = "Sve kategorije";
                    categoryAll.id = 0;
                    return categoryAll;
                }
            }
            set
            {
                if (value != null)
                {
                    IsolatedStorageSettings isoStore = IsolatedStorageSettings.ApplicationSettings;
                    if (isoStore.Contains("CategoryFilter"))
                    {
                        isoStore["CategoryFilter"] = value;
                    }
                    else
                    {
                        isoStore.Add("CategoryFilter", value);
                    }
                }
            }
        }

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        private static MyAdsViewModel myAdsViewModel;

        public static MyAdsViewModel MyAdsViewModel
        {
            get
            {
                if (myAdsViewModel == null)
                    myAdsViewModel = new MyAdsViewModel();

                return myAdsViewModel;
            }
        }

        private static NewAdViewModel newAdViewModel;

        public static NewAdViewModel NewAdViewModel
        {
            get
            {
                if (newAdViewModel == null)
                    newAdViewModel = new NewAdViewModel();

                return newAdViewModel;
            }
        }

        public static AdViewModel SelectedAd { get; set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                MetroGridHelper.IsVisible = true;
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadAllAdsAsync();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}