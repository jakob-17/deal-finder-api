using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealFinderApi.Models
{
    public class PriceListing
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string ItemPrice { get; set; }
        public string ItemTitle { get; set; }
    }
}
