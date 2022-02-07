using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
    }

}
