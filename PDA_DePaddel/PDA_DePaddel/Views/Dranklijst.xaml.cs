using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PDA_DePaddel.Models;
using System.Net;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using PDA_DePaddel.ViewModels;
using System.Collections.Specialized;

namespace PDA_DePaddel.Views
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dranklijst : Xamarin.Forms.TabbedPage
    {
        public List<Product> dranken { get; set; }
        public ObservableCollection<Grouping<string, Product>> dranks;
        public List<ProductOrder> bestellingDranks = new List<ProductOrder>();
        public Decimal Totaalprijs = 0;

        public Dranklijst()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<ProductListVM, String>(this, "ErrorProductList", (sender, args) =>
            {
                DisplayAlert("Error", "Something went wrong: " + args, "OK");
            });
        }
    }
}
