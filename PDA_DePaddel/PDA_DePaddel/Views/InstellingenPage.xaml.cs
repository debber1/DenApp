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
    public partial class InstellingenPage : ContentPage
    {
        public InstellingenPage()
        {

            InitializeComponent();
          
        }

        private void BtnTest_Clicked(object sender, EventArgs e)
        {

            WebClient client = new WebClient();
            Uri uri = new Uri("Not used");
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("test", "Test3");
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameters);

           
          
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                string output = Encoding.UTF8.GetString(e.Result);
                DisplayAlert("LOG", output + "!", "Oké");
            }
            catch (Exception ae)
            {
                Console.WriteLine("{0} Exception caught.", ae);

            }
        }
    }
}