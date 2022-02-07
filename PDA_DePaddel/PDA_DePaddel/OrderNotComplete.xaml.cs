using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDA_DePaddel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderNotComplete : PopupPage
    {
        public OrderNotComplete()
        {
            InitializeComponent();
        }
        private void BtnJa_Clicked(object sender, EventArgs e)
        {
            Services.ServerService.OrderDelivered();
            PopupNavigation.Instance.PopAsync(true);
        }

        private void BtnNee_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}