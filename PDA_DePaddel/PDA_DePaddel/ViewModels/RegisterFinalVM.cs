using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace PDA_DePaddel.ViewModels
{
    public class RegisterFinalVM : INotifyPropertyChanged
    {
        public ObservableCollection<ProductOrder> CurrentOrder { get; set; }
        public string TotalPrice { get; set; }
        public string Name { get; set; }
        public string OrderNumber { get; set; }
        public INavigation Navigation { get; set; }

        public RegisterFinalVM(INavigation navigation)
        {
            this.Navigation = navigation;

            CurrentOrder = Variables.ProductOrder;
            TotalPrice = Variables.TotalPrice.ToString();
            Name = Variables.Name;
            OrderNumber = Variables.OrderNumber.ToString();
            //Command for the done button
            DoneCommand = new Command(async () =>
            {
                Variables.ProductOrder = null;
                Variables.TotalPrice = 0;
                Variables.OrderNumber = 0;
                Variables.CurrentGuid = null;
                await Navigation.PopToRootAsync(true);
            });
        }

        public Command DoneCommand { get; }

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
