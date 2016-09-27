using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{


    /// <summary>
    /// Order head
    /// </summary>
    public class OrderVM
    {
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string CartId { get; set; }
        List<OrderDetail> OrderDetails { get; set; }

    }
}