using CoreFundamentals.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreFundamentals.Data
{
    public class CoreFundamentalsDbContext : DbContext
    {
        public CoreFundamentalsDbContext(DbContextOptions<CoreFundamentalsDbContext> options)
            : base(options)
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
