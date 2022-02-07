using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace PDA_DePaddel.ViewModels
{
    public class TokenFinalVM : INotifyPropertyChanged
    {
        public TokenFinalVM()
        {
            DoneCommand = new Command(async() =>
            {
                Variables.TokenOrder = null;
                Variables.TotalPrice = 0;

                await ((MainPage)App.Current.MainPage).Detail.Navigation.PopToRootAsync(true);

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TokenOrder> TokenOrder { get; set; } = Variables.TokenOrder;
        public string TotalPrice { get; set; } = Variables.TotalPrice.ToString();
        public string User { get; set; } = Variables.Name;

        public Command DoneCommand { get; }

        
    }
}
