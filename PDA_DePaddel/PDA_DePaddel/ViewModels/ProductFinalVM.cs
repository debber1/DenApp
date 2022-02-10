using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace PDA_DePaddel.ViewModels
{
    public class ProductFinalVM : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<ProductOrder> OrderProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public int OrderNumber { get; set; }
        public ProductFinalVM()
        {
            Name = Variables.Name;
            TotalPrice = Variables.TotalPrice;
            OrderNumber = Variables.OrderNumber;
            OrderProducts = Variables.ProductOrder;
            if (Variables.CalcOrderRev != null)
            {
                foreach (ProductOrder element in OrderProducts)
                {
                    element.DisplayAmount = element.Amount.ToString() + "=>" + Variables.CalcOrderRev[OrderProducts.IndexOf(element)].Amount.ToString();
                }
                Variables.CalcOrderRev = null;
            }
            else
            {
                foreach (ProductOrder element in OrderProducts)
                {
                    element.DisplayAmount = element.Amount.ToString();
                }
            }


            DoneCommand = new Command(Afgewerkt);
        }



        public Command DoneCommand { get; }


        public void Afgewerkt()
        {
            Services.ServerService.ProductDone();
        }

       


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
