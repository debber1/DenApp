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
using ZXing.Net.Mobile.Forms;
using System.Collections.ObjectModel;
using PDA_DePaddel.ViewModels;
using PDA_DePaddel.Services;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public List<HomeScreen> homescreens { get; set; }
        public MenuPage MenuPage { get; private set; }
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public ObservableCollection<OrderHead> bestellingen { get; set; }
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public bool magrefresh = false;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageVM(Navigation);
            MessagingCenter.Subscribe<HomePageVM, String>(this, "ErrorHomePage", (sender, args) =>
            {
                DisplayAlert("Error", "Something went wrong: " + args, "OK");
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        }
    }
}