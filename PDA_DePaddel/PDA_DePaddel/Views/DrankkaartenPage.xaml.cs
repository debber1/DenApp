using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
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
    public partial class DrankkaartenPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        

        public DrankkaartenPage()
        {
            InitializeComponent();            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Variables.Renew != true)
            {
                laden();
                Variables.Renew = true;

            }
            else
            {
                RootPage.NavigateFromMenu(0);
                Variables.Renew = false;
                MessagingCenter.Send(this, "OpenMenu");
            }


        }
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string output;
            try
            {
                output = Encoding.UTF8.GetString(e.Result);
                if (output == "0")
                {
                    DisplayAlert("fout", "Er is iets mis gelopen.", "OK");
                }
                else
                {
                    Navigation.PushAsync(new DrankkaartLijst(), true);
                }
            }
            catch (Exception ae)
            {
                DisplayAlert("Fout", "Web: Controleer uw netwerkverbinding: " + ae.Message, "oké");
            }
            finally
            {
                Activity.IsRunning = false;
            }
        }
        private void laden()
        {
            try
            {
                Activity.IsRunning = true;
                WebClient client = new WebClient();
                Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.LoadNewOrder);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_operator", Variables.GuidUser);
                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);
            }
            catch (WebException ex)
            {
                DisplayAlert("Fout", "Web: " + ex.Message, "oké");
                Activity.IsRunning = false;
            }
            catch (Exception ex)
            {
                DisplayAlert("Fout", "General: " + ex.Message, "oké");
                Activity.IsRunning = false;
            }
        }
    }
}