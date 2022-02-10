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
using PDA_DePaddel.Views;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;

namespace PDA_DePaddel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class drankpopup : PopupPage
    {
        public drankpopup()
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
            Variables.ProductOrder = null;
        }

        private void BtnDoorgaan_Clicked(object sender, EventArgs e)
        {

            if(Variables.Rev == -1)
            {
                try
                {
                    Activity1.IsRunning = true;
                    string json = JsonConvert.SerializeObject(Variables.ProductOrder);

                    WebClient client = new WebClient();
                    Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.UpLoadOrder);
                    NameValueCollection parameters = new NameValueCollection();
                    Variables.CurrentGuid = Guid.NewGuid().ToString();

                    parameters.Add("json", json);
                    parameters.Add("operator", Variables.GuidUser);
                    parameters.Add("guid", Variables.CurrentGuid);
                    parameters.Add("bestellingnr", Variables.OrderNumber.ToString());
                    parameters.Add("ID_evenement", Variables.EventGuid);
                    Variables.CurrentGuid = null;

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
            else
            {
                try
                {
                    Activity1.IsRunning = true;
                    string json = JsonConvert.SerializeObject(Variables.ProductOrder);

                    WebClient client = new WebClient();
                    Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.UploadRevision);
                    NameValueCollection parameters = new NameValueCollection();
                    Variables.CurrentGuid = Guid.NewGuid().ToString();
                    
                    
                    parameters.Add("ID_HEAD", Variables.RevCurrentGuid);
                    parameters.Add("guid", Variables.CurrentGuid);
                    parameters.Add("json", json);
                    parameters.Add("rev", (Variables.Rev+1).ToString());


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
                    ((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync(true);
                    PopupNavigation.Instance.PopAsync(true);
                    DisplayAlert("Succes", "Succesvol de bestelling door gegeven.", "Oké");
                    Variables.CurrentGuid = null;
                    Variables.RevCurrentGuid = null;
                    Variables.Rev = -1;
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