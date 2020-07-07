using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Example_repeat.Model
{
    public class OrderDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Confectionary> Confectionaries { get; set; }
        public DbSet<ConfectionaryOrder> ConfectionaryOrders { get; set; }

        public OrderDbContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee);
                entity.Property(e => e.IdEmployee).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.Employee).HasForeignKey(e => e.IdEmployee);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdCustomer);
                entity.Property(e => e.IdCustomer).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired();
                entity.HasMany(e => e.Orders).WithOne(e => e.Customer).HasForeignKey(e => e.IdCustomer);
            });

            modelBuilder.Entity<Confectionary>(entity =>
            {
                entity.HasKey(c => c.IdConfectionary);
                entity.Property(c => c.IdConfectionary).ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired();
                entity.HasMany(c => c.ConfectionaryOrders).WithOne(c => c.Confectionary).HasForeignKey(c => c.IdConfectionary);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.IdOrder);
                entity.Property(o => o.IdOrder).ValueGeneratedOnAdd();
                entity.Property(o => o.Notes).IsRequired();
                entity.HasMany(c => c.ConfectionaryOrders).WithOne(c => c.Order).HasForeignKey(c => c.IdOrder);
            });

            modelBuilder.Entity<ConfectionaryOrder>(entity =>
            {
                entity.HasKey(c => new { c.IdOrder, c.IdConfectionary });
                entity.Property(c => c.Quantity).IsRequired();
            });

        }


    }
}
