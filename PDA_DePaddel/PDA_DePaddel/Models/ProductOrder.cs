using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class ProductOrder
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
    }
}
