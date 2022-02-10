using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using Rg.Plugins.Popup.Pages;
namespace PDA_DePaddel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnnulerenPopup : PopupPage
    {
        public AnnulerenPopup()
        {
            InitializeComponent();
        }

        private void BtnJa_Clicked(object sender, EventArgs e)
        {
            ((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync(true);
            PopupNavigation.Instance.PopAsync(true);
            Variables.Operator = "";
            Variables.Name = "";
            Variables.Rev = -1;
        }

        private void BtnNee_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}