
using Newtonsoft.Json;
using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace PDA_DePaddel.Services
{
    public static class ServerService
    {

        static WebClient Client;
        static List<Token> Tokens { get; set; }
        static List<Product> Product { get; set; }
        static ObservableCollection<Grouping<string, Product>> Products;
        static ObservableCollection<OrderHead> Orders;
        static int OrderNumber;


        static ServerService()
        {
            Client = new WebClient();
        }

        public static int GetOrderNumber()
        {
            try
            {

                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.GetOrderNumber);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_evenement", Variables.EventGuid);
                byte[] response = Client.UploadValues(url, parameters);

                OrderNumber = Int32.Parse(Encoding.UTF8.GetString(response));
                if (OrderNumber != 0)
                {

                    Variables.OrderNumber = OrderNumber;
                }
                else
                {
                    Debug.WriteLine("Fout", "Fout bij het laden van de gegevens. KRITISHE FOUT! CONTACTEER BEHEERDER/ADMIN", "oké");
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return OrderNumber;
        }
        public static ObservableCollection<Grouping<string, Product>> GetProduct()
        {
            try
            {

                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.LoadProd);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_evenement", Variables.EventGuid);

                byte[] response = Client.UploadValues(url, parameters);

                String json = Encoding.UTF8.GetString(response);
                Product = JsonConvert.DeserializeObject<List<Product>>(json);

                var sorted = from drank in Product
                             orderby drank.Name
                             group drank by drank.Type into drankgroup
                             select new Grouping<string, Product>(drankgroup.Key, drankgroup);
                Products = new ObservableCollection<Grouping<string, Product>>(sorted);

            }
            catch (WebException ex)
            {
                Debug.WriteLine("Fout", "Fout bij het laden van de dranken: " + ex.Message, "oké");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fout", "General: " + ex.Message, "oké");
            }
            return Products;
        }
        public static List<Token> GetToken()
        {

            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.LoadToken);
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("ID_evenement", Variables.EventGuid);


                byte[] response = Client.UploadValues(url, parameters);

                string json = Encoding.UTF8.GetString(response);
                Tokens = JsonConvert.DeserializeObject<List<Token>>(json);

            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return Tokens;

        }
        public static void ProductDone()
        {
            try
            {
                Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.OrderProdDone);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("Guid", Variables.CurrentGuid);
                Client.UploadValuesCompleted += Client_UploadValuesCompleted;
                Client.UploadValuesAsync(uri, parameters);
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Fout", "Web: " + ex.Message, "oké");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fout", "General: " + ex.Message, "oké");
            }
        }
        private static async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        { // Moet nog in een try catch
            try
            {
                string output;
                output = Encoding.UTF8.GetString(e.Result);
                if (output == "0")
                {
                    //invoke a navigation popup
                    await PopupNavigation.Instance.PushAsync(new OrderNotComplete());
                }
                else if (output == "1")
                {

                    OrderDelivered();
                }
                else
                {
                    Debug.WriteLine("Fout", "Er ging iets mis.", "oké");
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Fout", "Web: " + ex.Message, "oké");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fout", "General: " + ex.Message, "oké");
            }

        }
        public static void OrderDelivered()
        {
            // verwijder alle data uit Variables

            Variables.ProductOrder = null;
            Variables.TotalPrice = 0;
            Variables.OrderNumber = 0;

            // zeggen dat alles afgeleverd is.
            try
            {
                Uri uri = new Uri(GlobalVariable.Base + GlobalVariable.OrderDelivered);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_Bestelling_kop", Variables.CurrentGuid);
                Client.UploadValues(uri, parameters);

                Variables.CurrentGuid = null;
                ((MainPage)App.Current.MainPage).Detail.Navigation.PopAsync(true);
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Fout", "Web: " + ex.Message, "oké");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fout", "General: " + ex.Message, "oké");
            }
        }
        public static ObservableCollection<OrderHead> GetOrderNotDone()
        {
            string output;
            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.OrderNotDone);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_operator", Variables.GuidUser);
                byte[] response = Client.UploadValues(url, parameters);
                output = Encoding.UTF8.GetString(response);
                switch (output)
                {
                    case "0":
                        Debug.WriteLine("Fout", "Er ging iets mis.", "oké");
                        break;
                    case "1":
                        break;
                    default:
                        Orders = JsonConvert.DeserializeObject<ObservableCollection<OrderHead>>(output);
                        Variables.OrderNotDone = Orders;
                        return Orders;
                        break;
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return null;
        }
        public static void LoadExistingOrder(Guid Idtaped)
        {
            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.LoadExistingOrder);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_bestelling", Idtaped.ToString());
                byte[] response = Client.UploadValues(url, parameters);

                string json = Encoding.UTF8.GetString(response);
                Variables.ProductOrder = JsonConvert.DeserializeObject<ObservableCollection<ProductOrder>>(json);

                Variables.Renew3 = true;
                decimal Totaalprijs = 0;
                foreach (ProductOrder element in Variables.ProductOrder)
                {
                    Totaalprijs = Totaalprijs + element.Price * element.Amount;
                }
                Variables.TotalPrice = Totaalprijs;
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
        }
        public static string LoadNewOrder()
        {
            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.LoadNewOrder);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_operator", Variables.GuidUser);
                byte[] response = Client.UploadValues(url, parameters);

                return Encoding.UTF8.GetString(response);
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return "0";
        }
        public static ObservableCollection<OrderHead> GetOrders()
        {

            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.LoadOrders);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("Guid", Variables.EventGuid);
                byte[] response = Client.UploadValues(url, parameters);
                string json = Encoding.UTF8.GetString(response);
                Orders = JsonConvert.DeserializeObject<ObservableCollection<OrderHead>>(json);
                return Orders;
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return null;
        }
        public static ObservableCollection<OrderHead> BarCompletion(Guid currentbestelling)
        {

            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.BarCompletion);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID_Bestelling_kop", currentbestelling.ToString());
                Client.UploadValues(url, parameters);
                return GetOrders();
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return null;
        }
        public static ObservableCollection<OrderDet> GetDetail(Guid Idtaped)
        {
            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.GetDetail);
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("ID", Idtaped.ToString());
                byte[] json = Client.UploadValues(url, parameters);
                string json2 = Encoding.UTF8.GetString(json);
                return JsonConvert.DeserializeObject<ObservableCollection<OrderDet>>(json2);
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return null;
        }
        public static List<HomeScreen> GetOperator(String Operator, String Event )
        {
            try
            {
                Uri url = new Uri(GlobalVariable.Base + GlobalVariable.GetOperator);
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("operator", Operator);
                parameters.Add("event_ID", Event);
                byte[] response = Client.UploadValues(url, parameters);

                string json = Encoding.UTF8.GetString(response);
                if(json == "0")
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<List<HomeScreen>>(json);
            }
            catch (WebException ex)
            {
                Debug.WriteLine("WebEx: " + ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Regular Ex: " + ex);
            }
            return null;
        }
    }
}
