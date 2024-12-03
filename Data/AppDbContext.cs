using CrediBill_ASP.Models;

namespace Credibill_ASP.Data
{
    using Credibill_ASP.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;

    namespace CrediBill_Web.Data
    {
        public class AppDbContext : IdentityDbContext<IdentityUser>
        {
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Invoice> Invoices { get; set; }
            public DbSet<Payment> Payments { get; set; }

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Configureren van relaties en query filters

                // Relatie: Customer - Invoice (1:N)
                modelBuilder.Entity<Customer>()
                    .HasMany(c => c.Invoices)
                    .WithOne(i => i.Customer)
                    .HasForeignKey(i => i.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relatie: Invoice - Payment (1:N)
                modelBuilder.Entity<Invoice>()
                    .HasMany(i => i.Payments)
                    .WithOne(p => p.Invoice)
                    .HasForeignKey(p => p.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Soft delete: Filteren van 'verwijderde' objecten
                modelBuilder.Entity<Customer>()
                    .HasQueryFilter(c => c.Deleted == DateTime.MaxValue);
                modelBuilder.Entity<Invoice>()
                    .HasQueryFilter(i => i.Deleted == DateTime.MaxValue);
                modelBuilder.Entity<Payment>()
                    .HasQueryFilter(p => p.Deleted == DateTime.MaxValue);
            }
        }
    }

}
