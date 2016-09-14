using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class AppUserVM 
    {

        [Required]
        [Display(Name ="User name: ")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Email address: ")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password: ")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Admin")]
        public bool Admin { get; set; }

        [Required]
        [Display(Name = "First name:")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name:")]
        public string LastName { get; set; }

        [Display(Name = "Phone number:")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address:")]
        public string Address { get; set; }

        [Display(Name = "Postal code:")]
        public string PostalCode { get; set; }

    }
}