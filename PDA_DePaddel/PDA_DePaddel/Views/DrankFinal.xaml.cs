using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PDA_DePaddel.Models;
using System.Net;
using System.Collections.Specialized;

namespace PDA_DePaddel.Views
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrankFinal : ContentPage
    {
        public bool belachelijk;
        public DrankFinal()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}