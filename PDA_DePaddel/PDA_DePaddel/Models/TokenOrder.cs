using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
   public class TokenOrder
    {
        public Guid ID { get; set; } 
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public int Amount { get; set; }

    }
   
}
