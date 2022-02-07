using PDA_DePaddel.Models;
using PDA_DePaddel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class kassafinal : ContentPage
    {
        public kassafinal()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = new RegisterFinalVM(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}