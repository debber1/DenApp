using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class Token
    {
        public Guid ID { get; set; }
        public Decimal Price { get; set; }
        public string Comment { get; set; }
    }
}
