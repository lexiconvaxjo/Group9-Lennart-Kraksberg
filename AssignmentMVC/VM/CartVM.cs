using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class CartVM
    {
        public int ID { get; set; }
        public string CartId { get; set; }
        public int ItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}