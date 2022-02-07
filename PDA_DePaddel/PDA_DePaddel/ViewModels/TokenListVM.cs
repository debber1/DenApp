using PDA_DePaddel.Models;
using PDA_DePaddel.Services;
using PDA_DePaddel.Views;
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
    public class TokenListVM : INotifyPropertyChanged
    {
        public List<Token> Tokens { get; set; }
        public ObservableCollection<TokenOrder> TokenOrder { get; set; }
        public Decimal TotalPrice { get; set; }
        public bool IsBusy { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string LastAmount { get; set; }

        public TokenListVM()
        {
            //Inladen van de drankkaarten in het begin.
            Tokens = ServerService.GetToken();
            Name = Variables.EventName;
            LastName = "Laatst gekozen item";
            LastAmount = "Hoeveelheid";
            IsBusy = false;
            TotalPrice = 0;
            TokenOrder = new ObservableCollection<TokenOrder>();

            //Command for the order button
            OrderCommand = new Command(async () =>
            {
                Variables.TotalPrice = TotalPrice;
                Variables.TokenOrder = TokenOrder;
                await PopupNavigation.Instance.PushAsync(new drankkaartpopup());
            });

            //Command for the abort button
            AbortCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new AnnulerenPopup());
            });
            
        }

        //Logic for when a Token gets tapped by the user.
        Token selectedToken;
        public Token SelectedToken
        {
            get => selectedToken;
            set
            {
                if(value != null)
                {
                    IsBusy = true;
                    NotifyPropertyChanged(nameof(IsBusy));
                    // Item toevoegen aan de bestelling 
                    if (TokenOrder.Count == 0)
                    {
                        // als er nog geen drankkaart aanwezig is
                        TokenOrder.Add(new TokenOrder()
                        {
                            ID = value.ID,
                            Price = value.Price,
                            Comment = value.Comment,
                            Amount = 1
                        });
                        // onderaan tonen wat er gekozen is.
                        int lastindex = TokenOrder.Count - 1;
                        LastName = TokenOrder[lastindex].Price.ToString();
                        LastAmount = TokenOrder[lastindex].Amount.ToString();
                    }
                    else
                    {
                        bool gevonden = false;
                        int Lindex = 0;
                        // forloop die bestdrankkaarts afloopt en die dan vergelijkt met itemindex
                        foreach (TokenOrder element in TokenOrder)
                        {
                            if (element.ID == value.ID)
                            {
                                // Amount +1 + Zeggen dat het iets gevonden heeft
                                Lindex = TokenOrder.IndexOf(element);
                                TokenOrder.Add(new TokenOrder()
                                {
                                    ID = value.ID,
                                    Price = value.Price,
                                    Comment = value.Comment,
                                    Amount = TokenOrder[Lindex].Amount + 1

                                });
                                TokenOrder.RemoveAt(Lindex);

                                int lastindex = TokenOrder.Count - 1;
                                LastName = TokenOrder[lastindex].Price.ToString();
                                LastAmount = TokenOrder[lastindex].Amount.ToString();

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
                            TokenOrder.Add(new TokenOrder()
                            {
                                ID = value.ID,
                                Price = value.Price,
                                Comment = value.Comment,
                                Amount = 1

                            });

                            int lastindex = TokenOrder.Count - 1;
                            LastName = TokenOrder[lastindex].Price.ToString();
                            LastAmount = TokenOrder[lastindex].Amount.ToString();

                        }

                    }
                    // Het volgende zorgt voor de totaalprijs berekening
                    TotalPrice = 0;
                    foreach (TokenOrder element in TokenOrder)
                    {
                        TotalPrice = TotalPrice + element.Price * element.Amount;
                    }

                    IsBusy = false;
                    value = null;
                }
                selectedToken = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(TotalPrice));
                NotifyPropertyChanged(nameof(LastAmount));
                NotifyPropertyChanged(nameof(LastName));
                NotifyPropertyChanged(nameof(TokenOrder));
                NotifyPropertyChanged(nameof(IsBusy));

            }
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

        public Command OrderCommand { get; }
        public Command AbortCommand { get; }
    }
}
