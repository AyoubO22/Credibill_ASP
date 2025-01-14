using CrediBill_ASP.Models;
using Microsoft.EntityFrameworkCore;
using Credibill_ASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


using System;
using GroupBudget_Web.ViewModels;

    namespace CrediBill_ASP.Data
    {
    public class AppDbContext : IdentityDbContext<CredibillUser>
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

        public DbSet<Credibill_ASP.Models.Language> Language { get; set; } = default;
        public DbSet<Credibill_ASP.Models.Parameter> Parameter { get; set; } = default!;
        public DbSet<GroupBudget_Web.ViewModels.UserViewModel> UserViewModel { get; set; }
    }
    }