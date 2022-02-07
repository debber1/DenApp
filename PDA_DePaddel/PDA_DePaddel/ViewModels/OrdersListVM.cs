using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PDA_DePaddel.ViewModels
{
    public class OrdersListVM : INotifyPropertyChanged
    {
        public ObservableCollection<OrderHead> Orders { get; set; }
        public ObservableCollection<OrderDet> OrderDetail { get; set; }
        public Guid Idtaped;
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        public bool IsRefreshing { get; set; }

        
        public OrdersListVM()
        {
            Name = Variables.EventName;
            IsBusy = false;
            Orders = Services.ServerService.GetOrders();
            Fixcolor();
            OrderDetail = new ObservableCollection<OrderDet>();

            //Command for the done button
            DoneCommand = new Command(async () =>
            {
                IsBusy = true;
                NotifyPropertyChanged(nameof(IsBusy));
                Guid currentbestelling = OrderDetail[0].ID_Order_Head;
                Orders = Services.ServerService.BarCompletion(currentbestelling);
                Fixcolor();
                NotifyPropertyChanged(nameof(Orders));
                IsBusy = false;
                NotifyPropertyChanged(nameof(IsBusy));
            });            
            //Command for the refresh
            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                NotifyPropertyChanged(nameof(IsRefreshing));
                Orders = Services.ServerService.GetOrders();
                Fixcolor();
                NotifyPropertyChanged(nameof(Orders));
                IsRefreshing = false;
                NotifyPropertyChanged(nameof(IsRefreshing));
            });
            //Refreshing every few secconds.
            Task.Run(async () =>
            {
                while(true)
                {
                    try
                    {
                        IsBusy = true;
                        NotifyPropertyChanged(nameof(IsBusy));

                        Orders = Services.ServerService.GetOrders();
                        Fixcolor();
                        NotifyPropertyChanged(nameof(Orders));

                        IsBusy = false;
                        NotifyPropertyChanged(nameof(IsBusy));
                        await Task.Delay(10000);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Regular Ex: " + ex);
                    }
                }
            });
        }


        OrderHead selectedOrder;
        public OrderHead SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if(value != null)
                {
                    IsBusy = true;
                    NotifyPropertyChanged(nameof(IsBusy));

                    OrderDetail = Services.ServerService.GetDetail(value.ID);
                    Orders = Services.ServerService.GetOrders();
                    Idtaped = value.ID;
                    Fixcolor();
                    NotifyPropertyChanged(nameof(OrderDetail));
                    NotifyPropertyChanged(nameof(Orders));

                    IsBusy = false;
                    NotifyPropertyChanged(nameof(IsBusy));
                    value = null;
                }
                selectedOrder = value;
                
            }
        }

        public Command DoneCommand { get; }
        public Command RefreshCommand { get; }

        public void Fixcolor()
        {
            // Zorgt dat de kleuren in orde blijven van de lijst.
            foreach (OrderHead element in Orders)
            {
                if (element.Done_Bar == 1 && element.Done_Table == 1 && element.ID != Idtaped)
                {
                    element.BGColour = System.Drawing.Color.Green;
                }
                else if (element.ID == Idtaped)
                {
                    element.BGColour = System.Drawing.Color.DeepSkyBlue;
                }
                else if (element.Done_Bar == 1)
                {
                    element.BGColour = System.Drawing.Color.DarkOrange;
                }
                else if (element.Done_Bar == 0 && element.Done_Table == 1)
                {
                    element.BGColour = System.Drawing.Color.BlueViolet;
                }
                else
                {
                    element.BGColour = System.Drawing.Color.Red;
                }
            }
            NotifyPropertyChanged(nameof(Orders));
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
