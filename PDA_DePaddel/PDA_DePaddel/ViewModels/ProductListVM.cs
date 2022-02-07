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
        public ObservableCollection<ProductOrder> ProductOrder { get; set; }
        public Decimal TotalPrice { get; set; }
        public bool IsBusy { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public string LastName { get; set; }
        public string LastAmount { get; set; }
        public ProductListVM()
        {
            Products = Services.ServerService.GetProduct();
            TotalPrice = 0;
            IsBusy = false;
            Name = Variables.Name;
            OrderNumber = Services.ServerService.GetOrderNumber();
            LastName = "Laatst gekozen item";
            LastAmount = "Hoeveelheid";
            ProductOrder = new ObservableCollection<ProductOrder>();

            //Command voor de bestel knop
            OrderCommand = new Command(async () =>
            {
                Variables.TotalPrice = TotalPrice;
                Variables.ProductOrder = ProductOrder;
                await PopupNavigation.Instance.PushAsync(new drankpopup());
            });

            //Command voor de anuleer knop
            AbortCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new AnnulerenPopup());
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
                    if (ProductOrder.Count == 0)
                    {
                        ProductOrder.Add(new ProductOrder()
                        {
                            // al er nog geen drankkaart aanwezig is
                            ID = value.ID,
                            Name = value.Name,
                            Price = value.Price,
                            Comment = value.Comment,
                            Amount = 1

                        });
                        // onderaan tonen wat er gekozen is
                        int lastindex = ProductOrder.Count - 1;
                        LastName = ProductOrder[lastindex].Name.ToString();
                        LastAmount = ProductOrder[lastindex].Amount.ToString();
                    }
                    else
                    {
                        bool gevonden = false;
                        int Lindex = 0;
                        // forloop die bestdranks afloopt en die dan vergelijkt met itemindex
                        foreach (ProductOrder element in ProductOrder)
                        {
                            if (element.ID == value.ID)
                            {
                                // Amount +1 + Zeggen dat het iets gevonden heeft
                                Lindex = ProductOrder.IndexOf(element);
                                ProductOrder.Add(new ProductOrder()
                                {
                                    ID = value.ID,
                                    Name = value.Name,
                                    Price = value.Price,
                                    Comment = value.Comment,
                                    Amount = ProductOrder[Lindex].Amount + 1

                                });
                                ProductOrder.RemoveAt(Lindex);
                                int lastindex = ProductOrder.Count - 1;
                                LastName = ProductOrder[lastindex].Name.ToString();
                                LastAmount = ProductOrder[lastindex].Amount.ToString();

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
                            ProductOrder.Add(new ProductOrder()
                            {
                                ID = value.ID,
                                Name = value.Name,
                                Price = value.Price,
                                Comment = value.Comment,
                                Amount = 1

                            });
                            int lastindex = ProductOrder.Count - 1;
                            LastName = ProductOrder[lastindex].Name.ToString();
                            LastAmount = ProductOrder[lastindex].Amount.ToString();
                        }
                    }
                    // zorgt voor de totaalprijs berekening
                    TotalPrice = 0;
                    foreach (ProductOrder element in ProductOrder)
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
                NotifyPropertyChanged(nameof(ProductOrder));
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }

        public Command OrderCommand { get; }
        public Command AbortCommand { get; }

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
