using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class OrderDetailVM
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}