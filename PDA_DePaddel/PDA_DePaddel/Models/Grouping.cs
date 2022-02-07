using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PDA_DePaddel.Models
{
   public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        } 

    }
}
