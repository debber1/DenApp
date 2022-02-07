using PDA_DePaddel.Models;
using PDA_DePaddel.ViewModels;
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
    public partial class kassapage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public kassapage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<RegisterListVM, String>(this, "ErrorRegisterList", (sender, args) =>
            {
                DisplayAlert("Error", "Something went wrong: " + args, "OK");
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Variables.Renew4 != true)
            {
                laden();
                Variables.Renew4 = true;

            }
            else
            {
                RootPage.NavigateFromMenu(0);
                Variables.Renew4 = false;
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
                    this.Navigation.PushAsync(new kassalijst(), true);
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