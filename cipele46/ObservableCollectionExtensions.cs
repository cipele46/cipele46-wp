﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace cipele46
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            foreach (var item in range)
                collection.Add(item);
        }
    }
}
