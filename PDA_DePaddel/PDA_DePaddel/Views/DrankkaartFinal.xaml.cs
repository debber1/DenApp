using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PDA_DePaddel.Models;
using PDA_DePaddel.ViewModels;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrankkaartFinal : ContentPage
    {
        public DrankkaartFinal()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}