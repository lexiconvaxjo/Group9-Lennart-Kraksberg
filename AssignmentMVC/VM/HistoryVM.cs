using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class HistoryVM
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public System.DateTime OrderDate { get; set; }

    }
}