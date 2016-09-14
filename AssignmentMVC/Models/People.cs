﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.Models
{
    public class People
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }



        public People()
        {

        }




        public People(string Name, string PhoneNumber, string City)
        {
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
            this.City = City;
        }


    }
}