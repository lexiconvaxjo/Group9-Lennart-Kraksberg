using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project01.VM
{
    public class ItemVM
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public int StockQty { get; set; }
    }
}