using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }

        public string CartId { get; set; }

        public int ItemId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public System.DateTime DateCreated { get; set; }

    }
}