using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{


    /// <summary>
    /// Product table
    /// </summary>
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public int StockQty { get; set; }

    }
}