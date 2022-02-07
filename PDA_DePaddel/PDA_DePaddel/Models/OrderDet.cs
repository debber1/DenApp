using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class OrderDet
    {
        public Guid ID { get; set; }
        public Guid ID_Order_Head { get; set; }
        public string Product { get; set; }
        public int Amount { get; set; }
    }
}
