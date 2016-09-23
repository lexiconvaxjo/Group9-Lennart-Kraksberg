using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{


    /// <summary>
    /// Order details
    /// </summary>
    public class OrderVM
    {
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public decimal Total { get; set; }
        List<OrderDetail> OrderDetails { get; set; }

    }
}