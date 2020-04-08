using EFCoreConApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreConApp.Models
{
    public class NorthWindContext : DbContext
    {
        public NorthWindContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}