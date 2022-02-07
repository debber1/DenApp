using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public enum MenuItemType
    {
        Home,
        Token,
        Order,
        Register,
        Settings,
        Orders
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

    }
}
