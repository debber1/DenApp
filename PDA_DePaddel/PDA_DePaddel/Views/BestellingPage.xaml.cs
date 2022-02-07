using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using PDA_DePaddel.ViewModels;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BestellingPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public List<OrderHead> bestellingen { get; set; }
        public BestellingPage()
        {
            InitializeComponent();
            BindingContext = new ProductPageVM(Navigation);
            MessagingCenter.Subscribe<ProductPageVM, String>(this, "ErrorProductPage", (sender, args) =>
            {
                DisplayAlert("Error", "Something went wrong: " + args, "OK");
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Variables.Renew3 == true)
            {
                RootPage.NavigateFromMenu(0);
                Variables.Renew3 = false;
                MessagingCenter.Send(this, "OpenMenu");
            }
        }
    }
}
