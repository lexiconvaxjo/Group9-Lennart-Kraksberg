using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string CityName { get; set; }

        public Country Countries { get; set; }
    }
}