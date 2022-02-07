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
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
