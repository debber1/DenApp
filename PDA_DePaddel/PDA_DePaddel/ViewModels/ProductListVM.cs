using PDA_DePaddel.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace PDA_DePaddel.ViewModels
{
    public class ProductListVM : INotifyPropertyChanged
    {
        public ObservableCollection<Grouping<string,Product>> Products { get; set; }
        public ObservableCollection<ProductOrder> ProductOrders { get; set; }
        public Decimal TotalPrice { get; set; }
        public bool IsBusy { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public string LastName { get; set; }
        public string LastAmount { get; set; }
        public bool RevisionMode = false;
        public ProductListVM()
        {
            Variables.ProductOrder = null;
            RevisionMode = false;
            Products = Services.ServerService.GetProduct();
            if(Products == null)
            {
                MessagingCenter.Send(this, "ErrorProductList", "Er ging iets mis bij het ophalen van de producten.");
            }
            TotalPrice = 0;
            IsBusy = false;
            Name = Variables.Name;
            
            if(OrderNumber == -1)
            {
                MessagingCenter.Send(this, "ErrorProductList", "Er ging iets mis bij het ophalen van het bestellingnummer.");
                Products = null;
            }
            LastName = "Laatst gekozen item";
            LastAmount = "Hoeveelheid";
            if (Variables.CalcOrderRev != null)
            {
                ProductOrders = Variables.CalcOrderRev;
                Variables.CalcOrderRev = null;
                RevisionMode = true;
                OrderNumber = Variables.OrderNumber;
            }
            else
            {
                Variables.Rev = -1;
                ProductOrders = new ObservableCollection<ProductOrder>();
                OrderNumber = Services.ServerService.GetOrderNumber();
            }

            //Command voor de bestel knop
            OrderCommand = new Command(async () =>
            {
                Variables.TotalPrice = TotalPrice;
                Variables.ProductOrder = ProductOrders;
                await PopupNavigation.Instance.PushAsync(new drankpopup());
            });

            //Command voor de anuleer knop
            AbortCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new AnnulerenPopup());
            });

            DeleteCommand = new Command((e) =>
            {
                var item = (e as ProductOrder);
                int index = ProductOrders.IndexOf(item);
                if (ProductOrders[index].Amount == 1)
                {
                    ProductOrders.Remove(item);
                }
                else
                {
                    ProductOrders[index].Amount--;
                    ProductOrders[index].OnPropertyChanged("Amount");
                    LastName = ProductOrders[index].Name.ToString();
                    LastAmount = ProductOrders[index].Amount.ToString();
                }

                TotalPrice = 0;
                foreach (ProductOrder element in ProductOrders)
                {
                    TotalPrice = TotalPrice + element.Price * element.Amount;
                }
                NotifyPropertyChanged(nameof(LastAmount));
                NotifyPropertyChanged(nameof(LastName));
                NotifyPropertyChanged(nameof(TotalPrice));
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ProductOrders));

            });
            AddCommand = new Command((e) =>
            {
                var item = e as ProductOrder;
                int index = ProductOrders.IndexOf(item);
                ProductOrders[index].Amount++;
                LastName = ProductOrders[index].Name.ToString();
                LastAmount = ProductOrders[index].Amount.ToString();
                TotalPrice = 0;
                foreach (ProductOrder element in ProductOrders)
                {
                    TotalPrice = TotalPrice + element.Price * element.Amount;
                }
                NotifyPropertyChanged(nameof(LastAmount));
                NotifyPropertyChanged(nameof(LastName));
                NotifyPropertyChanged(nameof(TotalPrice));
                NotifyPropertyChanged(nameof(ProductOrders));
                NotifyPropertyChanged();
                ProductOrders[index].OnPropertyChanged("Amount");
            });
        }


        Product selectedProduct;
        public Product SelectedProduct 
        {
            get => selectedProduct;
            set
            {
                if(value != null)
                {
                    IsBusy = true;
                    NotifyPropertyChanged(nameof(IsBusy));
                    if (ProductOrders.Count == 0)
                    {
                        ProductOrders.Add(new ProductOrder()
                        {
                            // al er nog geen drankkaart aanwezig is
                            ID = value.ID,
                            Name = value.Name,
                            Price = value.Price,
                            Comment = value.Comment,
                            Amount = 1

                        });
                        // onderaan tonen wat er gekozen is
                        int lastindex = ProductOrders.Count - 1;
                        LastName = ProductOrders[lastindex].Name.ToString();
                        LastAmount = ProductOrders[lastindex].Amount.ToString();
                    }
                    else
                    {
                        bool gevonden = false;
                        int Lindex = 0;
                        // forloop die bestdranks afloopt en die dan vergelijkt met itemindex
                        foreach (ProductOrder element in ProductOrders)
                        {
                            if (element.ID == value.ID)
                            {
                                // Amount +1 + Zeggen dat het iets gevonden heeft
                                Lindex = ProductOrders.IndexOf(element);
                                ProductOrders.Add(new ProductOrder()
                                {
                                    ID = value.ID,
                                    Name = value.Name,
                                    Price = value.Price,
                                    Comment = value.Comment,
                                    Amount = ProductOrders[Lindex].Amount + 1

                                });
                                ProductOrders.RemoveAt(Lindex);
                                int lastindex = ProductOrders.Count - 1;
                                LastName = ProductOrders[lastindex].Name.ToString();
                                LastAmount = ProductOrders[lastindex].Amount.ToString();

                                gevonden = true;
                            }
                            if (gevonden == true)
                            {
                                break;
                            }
                        }
                        // als de drankkaart nog niet in de lijst stond
                        if (gevonden != true)
                        {
                            ProductOrders.Add(new ProductOrder()
                            {
                                ID = value.ID,
                                Name = value.Name,
                                Price = value.Price,
                                Comment = value.Comment,
                                Amount = 1

                            });
                            int lastindex = ProductOrders.Count - 1;
                            LastName = ProductOrders[lastindex].Name.ToString();
                            LastAmount = ProductOrders[lastindex].Amount.ToString();
                        }
                    }
                    // zorgt voor de totaalprijs berekening
                    TotalPrice = 0;
                    foreach (ProductOrder element in ProductOrders)
                    {
                        TotalPrice = TotalPrice + element.Price * element.Amount;
                    }

                    IsBusy = false;
                    value = null;
                }
                selectedProduct = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TotalPrice));
                NotifyPropertyChanged(nameof(LastAmount));
                NotifyPropertyChanged(nameof(LastName));
                NotifyPropertyChanged(nameof(ProductOrders));
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }

        public Command OrderCommand { get; }
        public Command AbortCommand { get; }
        public Command AddCommand { get; }
        public Command DeleteCommand { get; }

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
