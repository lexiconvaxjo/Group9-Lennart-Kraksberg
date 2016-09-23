using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{

    /// <summary>
    /// Order detail
    /// </summary>
    public class OrderDetail 
    {
        [Key]
        public int OrderDetailId { get; set; }
      
        public int OrderId { get; set; }
       
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

    }
}