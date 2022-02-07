using Newtonsoft.Json;
using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Linq;

namespace PDA_DePaddel.ViewModels
{
    public class GroupingVM
    {
        public ObservableCollection<Product> Dranken { get; set; }
        public ObservableCollection<Grouping<string, Product>> Dranken_grouped { get; set; }
       
        public GroupingVM()
        {

        }
        public GroupingVM(bool designData)
        {
            if (designData)
            {
                loaddrank();
                Dranken = Dranken;
                var sorted = from drank in Dranken
                             orderby drank.Name
                             group drank by drank.Type into drankgroup
                             select new Grouping<string, Product>(drankgroup.Key, drankgroup);

                Dranken_grouped = new ObservableCollection<Grouping<string, Product>>(sorted);

            }
        }
        public bool IsBusy { get; set; }

        public void loaddrank()
        {
            try
            {
           
                WebClient mclient = new WebClient();
                Uri url = new Uri("http://debapidev.eu/loaddrank.php");

                mclient.DownloadDataAsync(url);
                mclient.DownloadDataCompleted += Mclient_DownloadDataCompleted;


            }
            catch (WebException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void Mclient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            Dranken = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
        }
    }
}
