using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class Product_Group : List<Product>
    {
        public string Type { get; set; }

        public Product_Group(string type)
        {
            Type = type;
        }
        public static IList<Product_Group> Products { set; get; }
    }
}
