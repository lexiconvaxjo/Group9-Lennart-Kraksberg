using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{

    /// <summary>
    /// Order head
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }

        public decimal Total  { get; set; }
        public string CartId { get; set; }

        List<OrderDetail> OrderDetails { get; set; }

    }
}