using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaAndBeautyWebsite.Models;

namespace SpaAndBeautyWebsite.Data
{
    public class SpaAndBeautyWebsiteContext : DbContext
    {
        public SpaAndBeautyWebsiteContext (DbContextOptions<SpaAndBeautyWebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<SpaAndBeautyWebsite.Models.Appointment> Appointment { get; set; } = default!;
        public DbSet<SpaAndBeautyWebsite.Models.Customer> Customer { get; set; } = default!;
        public DbSet<SpaAndBeautyWebsite.Models.Review> Review { get; set; } = default!;
        public DbSet<SpaAndBeautyWebsite.Models.Service> Service { get; set; } = default!;
        public DbSet<SpaAndBeautyWebsite.Models.Employee> Employee { get; set; } = default!;
    }
}
