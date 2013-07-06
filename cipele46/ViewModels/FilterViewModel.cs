using cipele46.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cipele46.ViewModels
{
    public class FilterViewModel : ViewModelBaseEx
    {
        private bool _isDataLoading;
        private bool _isDataLoaded;
        private county _countyFilter;
        private category _categoryFilter;

        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set { Set(ref _isDataLoading, value); }
        }
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { Set(ref _isDataLoaded, value); }
        }

        public county CountyFilter
        {
            get { return _countyFilter; }
            set { Set(ref _countyFilter, value); }
        }

        public category CategoryFilter
        {
            get { return _categoryFilter; }
            set { Set(ref _categoryFilter, value); }
        }

        public ObservableCollection<category> Categories { get; set; }
        public ObservableCollection<county> Counties { get; set; }

        public FilterViewModel()
        {
            Categories = new ObservableCollection<category>();
            Counties = new ObservableCollection<county>();
            CategoryFilter = ((App)Application.Current).CategoryFilter;
            CountyFilter = ((App)Application.Current).CountyFilter;
        }

        public async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            foreach (var category in await App.GetCategoriesAsync())
                Categories.Add(category);

            foreach (var county in await App.GetCountiesAsync())
                Counties.Add(county);

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
