﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cipele46
{
    public static class Endpoints
    {
        // static string apiUrl = "http://cipele46.org/api";
        private static string apiUrl = "http://staging.cipele46.org/api";

        public static readonly string AdsUrl = apiUrl + "/ads";
        public static readonly string CategoriesUrl = apiUrl + "/categories.json";
        public static readonly string CountiesUrl = apiUrl + "/regions.json";
        public static readonly string RegisterUserUrl = apiUrl + "/users.json";
        public static readonly string LoginUserUrl = apiUrl + "/users/current.json";
        public static readonly string ToggleFavoriteUrl = apiUrl + "/ads/{0}/toggle_favorite";
        public static readonly string ReplyToAddUrl = apiUrl + "/ads/{0}/reply";
        public static readonly string PostAnAdUrl = apiUrl + "/ads";

    }
}
