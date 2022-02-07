using Newtonsoft.Json;
using PDA_DePaddel.Models;
using PDA_DePaddel.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class kassalijst : TabbedPage
    {

        public kassalijst()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<RegisterListVM, String>(this, "ErrorRegisterList", (sender, args) =>
            {
                DisplayAlert("Error", "Something went wrong: " + args, "OK");
            });
        }
        
    }
}