using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class LogInVM 
    {

        [Required]
        [Display(Name = "User name: ")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
    }
}