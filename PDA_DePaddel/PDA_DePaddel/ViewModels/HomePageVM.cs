using PDA_DePaddel.Models;
using PDA_DePaddel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace PDA_DePaddel.ViewModels
{
    public class HomePageVM : INotifyPropertyChanged
    {
        public bool LoginPageVisable { get; set; }
        public bool HomePageVisable { get; set; }
        public ObservableCollection<OrderHead> Orders { get; set; }
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public List<HomeScreen> HomeScreens { get; set; }
        public string EventName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string EventID { get; set; }
        public string EventNews { get; set; }
        public INavigation Navigation { get; set; }
        public bool IsBusy { get; set; }
        public string txtOperator { get; set; }
        public string txtEvent { get; set; }

        public HomePageVM(INavigation navigation)
        {
            this.Navigation = navigation;
            Orders = new ObservableCollection<OrderHead>();
            HomeScreens = new List<HomeScreen>();
            LoginPageVisable = true;
            HomePageVisable = false;



            //Command for the new OK button
            OKCommand = new Command(async () =>
            {
                HomeScreens = Services.ServerService.GetOperator(txtOperator, txtEvent);
                if (HomeScreens == null)
                {
                    Debug.WriteLine("fout", "Dit operator nummer bestaat niet!", "OK");
                    MessagingCenter.Send(this, "ErrorHomePage", "Dit operator nummer bestaat niet!");
                }
                else
                {
                    IsBusy = true;
                    NotifyPropertyChanged(nameof(IsBusy));
                    Variables.Name = HomeScreens[0].Name;
                    Variables.EventName = HomeScreens[0].LastName;
                    Variables.NFC_Enable = HomeScreens[0].NFC_Enable;
                    Variables.Photo = HomeScreens[0].Image;
                    Variables.EventGuid = HomeScreens[0].ID;
                    Variables.Pin = HomeScreens[0].Pin;
                    Variables.GuidUser = HomeScreens[0].ID_User;
                    MessagingCenter.Send(this, "OpenMenu");

                    Name = HomeScreens[0].Name;
                    LastName = HomeScreens[0].LastName;
                    EventName = HomeScreens[0].EventName;
                    EventID = txtEvent;
                    EventNews = HomeScreens[0].EventNews;
                    LoginPageVisable = false;
                    HomePageVisable = true;
                    Orders = Services.ServerService.GetOrderNotDone();
                    if (Orders == null)
                    {
                        MessagingCenter.Send(this, "ErrorHomePage", "Fout bij het herladen van de bestellingen.");
                    }
                    else
                    {
                        fixcolor();
                        Variables.OrderNotDone = Orders;
                    }
                    NotifyPropertyChanged(nameof(Name));
                    NotifyPropertyChanged(nameof(LastName));
                    NotifyPropertyChanged(nameof(EventName));
                    NotifyPropertyChanged(nameof(EventID));
                    NotifyPropertyChanged(nameof(EventNews));
                    NotifyPropertyChanged(nameof(LoginPageVisable));
                    NotifyPropertyChanged(nameof(HomePageVisable));
                    NotifyPropertyChanged(nameof(Orders));
                    IsBusy = false;
                    NotifyPropertyChanged(nameof(IsBusy));
                }
            });
            //Command for the log off button
            LogOffCommand = new Command(async () =>
            {
                LoginPageVisable = true;
                HomePageVisable = false;
                Variables.Pin = 0;
                Variables.NFC_Enable = 0;
                Variables.Name = "gebruiker";
                Variables.EventName = "gebruiker";
                Variables.GuidUser = "";
                MessagingCenter.Send(this, "OpenMenu");
                Variables.Photo = "";
                Variables.EventGuid = "";
                NotifyPropertyChanged(nameof(LoginPageVisable));
                NotifyPropertyChanged(nameof(HomePageVisable));

            });
            //Command for the QR button
            QRCommand = new Command(async () =>
            {
                var scanpage = new ZXingScannerPage();
                // Navigation to the scanning page
                await Navigation.PushAsync(scanpage);

                // Waiting for the result of the scan
                scanpage.OnScanResult += (result) =>
                {
                    // Stop scanning
                    scanpage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //this happens when the qr code get scanned
                        await Navigation.PopAsync();
                        //await DisplayAlert("Scanned Barcode", result.Text, "OK");

                        // adding the result to the event text box
                        txtEvent = result.Text;
                        NotifyPropertyChanged(nameof(txtEvent));
                    });
                };
            });
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10000);
                    switch (HomePageVisable)
                    {
                        case true:
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsBusy = true;
                                NotifyPropertyChanged(nameof(IsBusy));
                                Orders = Services.ServerService.GetOrderNotDone();
                                if (Orders == null)
                                {
                                    MessagingCenter.Send(this, "ErrorHomePage", "Fout bij het herladen van de bestellingen.");
                                }
                                else
                                {
                                    fixcolor();
                                    Variables.OrderNotDone = Orders;
                                }
                                NotifyPropertyChanged(nameof(Orders));
                                IsBusy = false;
                                NotifyPropertyChanged(nameof(IsBusy));
                            });

                            break;
                        case false:

                            break;
                    }
                }
            });
        }
        OrderHead selectedOrder;
        public OrderHead SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if (value != null)
                {
                    MessagingCenter.Send<HomePageVM, OrderHead>(this, "bestelling", value);
                    // dit werkt niet voor de eerste keer na opstart omdat de bestellingpagina nog niet ingeladen is, en dus ook nog geen functies kan doen lopen.
                    MessagingCenter.Send<HomePageVM>(this, "opendrank");
                    value = null;
                }
                selectedOrder = value;
                NotifyPropertyChanged();

            }
        }

        public Command QRCommand { get; }
        public Command OKCommand { get; }
        public Command LogOffCommand { get; }
        public void fixcolor()
        {
            // Zorgt dat de kleuren in orde blijven van de lijst.
            foreach (OrderHead element in Orders)
            {
                if (element.Done_Bar == 1)
                {
                    element.BGColour = System.Drawing.Color.Green;
                }
                else
                {
                    element.BGColour = System.Drawing.Color.Red;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
