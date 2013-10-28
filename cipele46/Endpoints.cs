using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipele46
{
    public static class Endpoints
    {
        //private static string apiUrl = "http://cipele46.org";
        private static string apiUrl = "http://staging.cipele46.org/api";

        public static readonly string AdsUrl = apiUrl + "/ads";
        public static readonly string CategoriesUrl = apiUrl + "/categories.json";
        public static readonly string CountiesUrl = apiUrl + "/regions.json";
        public static readonly string RegisterUserUrl = apiUrl + "/users.json";
        public static readonly string LoginUserUrl = apiUrl + "/users/current.json";

    }
}
