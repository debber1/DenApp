using PDA_DePaddel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDA_DePaddel.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
            
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Token:
                        MenuPages.Add(id, new NavigationPage(new DrankkaartenPage()));
                        break;
                    case (int)MenuItemType.Order:
                        MenuPages.Add(id, new NavigationPage(new BestellingPage()));
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new InstellingenPage()));
                        break;
                    case (int)MenuItemType.Orders:
                        MenuPages.Add(id, new NavigationPage(new BestellingenPage()));
                        break;
                    case (int)MenuItemType.Register:
                        MenuPages.Add(id, new NavigationPage(new kassapage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }


        }
        public void OnToggleRequest()
        {
            IsPresented = !IsPresented;
        }
    }
}