using cipele46.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipele46.ViewModels
{
    public class FilterViewModel : ViewModelBaseEx
    {
        private bool _isDataLoading;
        private bool _isDataLoaded;

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

        public ObservableCollection<category> Categories { get; set; }

        public FilterViewModel()
        {
            Categories = new ObservableCollection<category>();
        }

        public async Task LoadDataAsync()
        {
            if (IsDataLoading)
                return;
            IsDataLoading = true;

            await App.GetCategoriesAsync();
            foreach (var category in App.Categories)
                Categories.Add(category);

            IsDataLoading = false;
            IsDataLoaded = true;
        }
    }
}
