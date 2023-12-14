using EcomTest.Domain.DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Common.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Gives StockItem a property for concurrency checking, the value that it has been read with is passed to the Save EF functionality, and if it differs, it knows it has been / is being read elsewhere so you can hadle the action what you want to happen - default is to throw an exception - can use it like a ticket booking system if needed to manage customer experience / UX
            modelBuilder.Entity<StockItem>().Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}
