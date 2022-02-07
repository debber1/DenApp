using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class OrderHead
    {
        public Guid ID { get; set; }
        public string User { get; set; }
        public int OrderNumber { get; set; }
        public DateTime Creation { get; set; }
        public int Done_Bar { get; set; }
        public int Done_Table { get; set; }
        public int Register { get; set; }
        public string Comment { get; set; }
        public Color BGColour { get; set; }

    }
}
