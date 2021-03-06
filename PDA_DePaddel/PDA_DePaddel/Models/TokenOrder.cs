using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PDA_DePaddel.Models
{
   public class TokenOrder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guid ID { get; set; } 
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public int Amount { get; set; }

    }
   
}
