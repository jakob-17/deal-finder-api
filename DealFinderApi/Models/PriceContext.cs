using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealFinderApi.Models
{
    public class PriceContext : DbContext
    {
        public PriceContext(DbContextOptions<PriceContext> options)
            : base(options)
        {
        }

        public DbSet<PriceListing> PriceListings { get; set; }
    }
}
