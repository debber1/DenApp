using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
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
    public class ProductPageVM : INotifyPropertyChanged
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public ObservableCollection<OrderHead> Orders { get; set; }
        public INavigation Navigation { get; set; }
        
        public ProductPageVM(INavigation navigation)
        {
            this.Navigation = navigation;
            if(Variables.Renew3 != true)
            {
                Orders = Services.ServerService.GetOrderNotDone();
                if(Orders != null)
                {
                    fixcolor();
                }
                else
                {
                    MessagingCenter.Send(this, "ErrorProductPage", "Er ging iets mis bij het ophalen van de bestellingen.");
                }
            }
            else
            {
                RootPage.NavigateFromMenu(0);
                Variables.Renew3 = false;
                MessagingCenter.Send(this, "OpenMenu");
            }
            //making it possible to open orders from HomePage
            MessagingCenter.Subscribe<HomePageVM, OrderHead>(this, "bestelling", (sender, arg) =>
            {
                Guid Idtaped = arg.ID;
                Variables.CurrentGuid = Idtaped.ToString();
                Variables.OrderNumber = arg.OrderNumber;

                LoadOrderDetail(Idtaped);
            });
            //Command for the new order button
            NewOrderCommand = new Command(async () =>
            {
                try
                {
                    if(Services.ServerService.LoadNewOrder() == "0")
                    {
                        Debug.WriteLine("fout", "Dit operator nummer bestaat niet!", "OK");
                        MessagingCenter.Send(this, "ErrorProductPage", "Dit operator nummer bestaat niet!");
                    }
                    else
                    {
                        await Navigation.PushAsync(new Dranklijst(), true);
                        Variables.Renew3 = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception: " + ex);
                }
            });
            //Command for the new order button
            EditCommand = new Command( (e) =>
            {
                var item = (e as OrderHead);
                try
                {
                    Services.ServerService.LoadExistingOrder(item.ID);
                    Variables.RevCurrentGuid = item.ID.ToString();
                    Variables.OrderNumber = item.OrderNumber;
                    Variables.Rev = item.Rev;
                    Navigation.PushAsync(new Dranklijst(), true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception: " + ex);
                    MessagingCenter.Send(this, "ErrorProductPage", "Er ging iets mis bij het ophalen van de bestelling.");
                }

            });
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10000);
                    if(Variables.OrderNotDone != null)
                    {
                        Orders = Variables.OrderNotDone;
                        NotifyPropertyChanged(nameof(Orders));
                    }
                }
            });
        }


        public Command NewOrderCommand { get; }
        public Command EditCommand { get; }

        OrderHead selectedOrder;
        public OrderHead SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if(value != null)
                {
                    Guid Idtaped = value.ID;
                    Variables.CurrentGuid = Idtaped.ToString();
                    Variables.OrderNumber = value.OrderNumber;

                    LoadOrderDetail(Idtaped);
                    value = null;
                }
                selectedOrder = value;
                NotifyPropertyChanged();

            }
        }
        public void LoadOrderDetail(Guid Idtaped)
        {
            try
            {
                Services.ServerService.LoadExistingOrder(Idtaped);
                Navigation.PushAsync(new DrankFinal(), true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception: " + ex);
                MessagingCenter.Send(this, "ErrorProductPage", "Er ging iets mis bij het ophalen van de bestelling.");
            }
        }

        public void fixcolor()
        {
            // Zorgt dat de kleuren in orde blijven van de lijst.
            foreach (OrderHead element in Orders)
            {
                if (element.Done_Bar == 1)
                {
                    element.BGColour = System.Drawing.Color.Green;
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
