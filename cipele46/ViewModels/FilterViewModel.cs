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
        }

        public async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            List<category> categories = new List<category>(App.Categories);
            if (categories == null)
            {
                categories = await App.GetCategoriesAsync();
                
            }
            category categoryAll = new category();
            categoryAll.name = "Sve kategorije";
            categoryAll.id = 0;
            categories.Insert(0, categoryAll);

            foreach (var category in categories)
                Categories.Add(category);

            List<county> counties = new List<county>(App.Counties);
            if (counties == null)
            {
                counties = await App.GetCountiesAsync();
                
            }

            county countyAll = new county();
            countyAll.name = "Sve županije";
            countyAll.id = 0;
            counties.Insert(0, countyAll);

            foreach (var county in counties)
                Counties.Add(county);

            CategoryFilter = Categories.FirstOrDefault(i => i != null && i.id == ((App)Application.Current).CategoryFilter.id);
            CountyFilter = Counties.FirstOrDefault(i => i != null && i.id == ((App)Application.Current).CountyFilter.id);

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
