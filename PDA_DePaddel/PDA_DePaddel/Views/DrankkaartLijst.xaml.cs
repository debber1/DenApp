using Android.Widget;
using Newtonsoft.Json;
using PDA_DePaddel.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Rg.Plugins.Popup.Services;
using System.Threading;
using System.Collections.Specialized;
using PDA_DePaddel.ViewModels;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrankkaartLijst : Xamarin.Forms.TabbedPage
    {
        public DrankkaartLijst()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<TokenListVM,String>(this, "ErrorTokenList", (sender,args) =>
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