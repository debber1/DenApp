using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PDA_DePaddel.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Specialized;
using System.Threading.Tasks;
using PDA_DePaddel.ViewModels;

namespace PDA_DePaddel.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BestellingenLijst : TabbedPage
    {
        public List<OrderHead> bestellingen { get; set; }
        public List<OrderDet> bestellingendetail { get; set; }
        public Guid Idtaped;
        OrdersListVM ViewModel => BindingContext as OrdersListVM;

        public BestellingenLijst()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<OrdersListVM>(this, "SwitchTabDetail", (sender) =>
            {
                this.CurrentPage = this.DetailTab;
                
            });

            MessagingCenter.Subscribe<OrdersListVM>(this, "SwitchTabMain", (sender) =>
            {
                this.CurrentPage = this.MainTab;
                DisplayAlert("Info", "Succesvol doorgegeven.", "OK");
            });
            MessagingCenter.Subscribe<OrdersListVM, String>(this, "ErrorOrdersList", (sender, args) =>
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