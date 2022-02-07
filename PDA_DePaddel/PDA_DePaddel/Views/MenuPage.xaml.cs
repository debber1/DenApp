using PDA_DePaddel.Models;
using PDA_DePaddel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace PDA_DePaddel.Views
{
   
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MenuPage : ContentPage
    {

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        

        List<HomeMenuItem> menuItems;
    
        
        public MenuPage()
        {
            InitializeComponent();
            Label_gebruiker.Text = Variables.Name;
            if (Variables.Photo != "")
            {
                byte[] imageBytes = Convert.FromBase64String(Variables.Photo);
                Stream stream = new MemoryStream(imageBytes);
                var imageSource = ImageSource.FromStream(() => stream);
                image.Source = imageSource;
            }
            else
            {
                image.Source = "@drawable/placeholder_debeah";
            }

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home", Icon="@drawable/home" },
                new HomeMenuItem {Id = MenuItemType.Token, Title="Drankkaarten", Icon="@drawable/drankkaarten" },
                new HomeMenuItem {Id = MenuItemType.Order, Title="Bestelling", Icon="@drawable/bestelling" },
             //   new HomeMenuItem {Id = MenuItemType.Instellingen, Title="Instellingen", Icon="@drawable/instellingen" },
                new HomeMenuItem {Id = MenuItemType.Orders, Title="Bestellingen", Icon="@drawable/altimeter"},
                new HomeMenuItem {Id = MenuItemType.Register, Title="Kassa", Icon="@drawable/baseline_point_of_sale_black_18dp" }
            };


            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                //dit gebeurt er als je op een menu item klikt
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
                
            };
            MessagingCenter.Subscribe<DrankkaartenPage>(this, "OpenMenu", (sender) =>
            {
                ListViewMenu.SelectedItem = menuItems[0];
            });
            MessagingCenter.Subscribe<kassapage>(this, "OpenMenu", (sender) =>
            {
                ListViewMenu.SelectedItem = menuItems[0];
            });
            MessagingCenter.Subscribe<HomePageVM>(this, "OpenMenu", (sender) =>
            {
                laden();
                ListViewMenu.SelectedItem = menuItems[0];
            });
            MessagingCenter.Subscribe<BestellingenPage>(this, "OpenMenu", (sender) =>
            {
                ListViewMenu.SelectedItem = menuItems[0];
            });
            MessagingCenter.Subscribe<BestellingPage>(this, "OpenMenu", (sender) =>
            {
                ListViewMenu.SelectedItem = menuItems[0];
            });
            MessagingCenter.Subscribe<HomePageVM>(this, "opendrank", (sender) =>
            {
                RootPage.NavigateFromMenu(2);
                ListViewMenu.SelectedItem = menuItems[2];
            });
        }
        

        
        public void laden()
        {
            Label_gebruiker.Text = Variables.Name;
            if (Variables.Photo != "")
            {
                byte[] imageBytes = Convert.FromBase64String(Variables.Photo);
                Stream stream = new MemoryStream(imageBytes);
                var imageSource = ImageSource.FromStream(() => stream);
                image.Source = imageSource;
            }
            else
            {
                image.Source = "@drawable/placeholder_debeah";
            }
        }
    }
}