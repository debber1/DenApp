using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using PDA_DePaddel.Models;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;

namespace PDA_DePaddel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class drankkaartpopup : PopupPage
    {
        public drankkaartpopup()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            // inladen van de prijs
            base.OnAppearing();
            txtprijs.Text = "Totaalprijs: " + Variables.TotalPrice.ToString();
        }

        private void BtnAnnuleer_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
            Variables.TotalPrice = 0;
            Variables.TokenOrder = null;
            
        }

        private void BtnDoorgaan_Clicked(object sender, EventArgs e)
        { 
            try
            {
                Activity1.IsRunning = true;
                string json = JsonConvert.SerializeObject(Variables.TokenOrder);

                WebClient client = new WebClient();
                Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.UpLoadToken);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("json", json);
                parameters.Add("operator", Variables.GuidUser);
                parameters.Add("ID_evenement", Variables.EventGuid);

                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);

            }
            catch (WebException ex)
            {
                DisplayAlert("Fout", "Web: " + ex.Message, "oké");
                Activity1.IsRunning = false;
            }
            catch (Exception ex)
            {
                DisplayAlert("Fout", "General: " + ex.Message, "oké");
                Activity1.IsRunning = false;
            }
            finally
            {
                PopupNavigation.Instance.PopAsync(true);
            }

        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                string output;
                output = Encoding.UTF8.GetString(e.Result);

                if (output == "0")
                {
                    DisplayAlert("Fout", "Fout bij het uploaden van de gegevens.", "oké");
                }
                else
                {
                    //Application.Current.MainPage.Navigation.PushAsync(new DrankkaartFinal());
                    ((MainPage)App.Current.MainPage).Detail.Navigation.PushAsync(new DrankkaartFinal());
                    DisplayAlert("Succes", "Succesvol de bestelling door gegeven.", "Oké");
                }
            }
            catch (WebException ex)
            {
                DisplayAlert("Fout", "Fout bij het laden van de bestellingen: " + ex.Message, "oké");
            }
            catch (Exception ex)
            {
                DisplayAlert("Fout", "General: " + ex.Message, "oké");
            }
            finally
            {
                Activity1.IsRunning = false;
            }
        }
    }
}