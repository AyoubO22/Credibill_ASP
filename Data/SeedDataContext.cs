using Credibill_ASP.Data.CrediBill_Web.Data;
using Credibill_ASP.Models;
using CrediBill_ASP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Host;

namespace CrediBill_ASP.Data
{
    public static class SeedDataContext
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Zorg dat de database bestaat
                context.Database.EnsureCreated();

                // Voeg enkele Customers toe als ze nog niet bestaan
                if (!context.Customers.Any())
                {
                    var customer1 = new Customer
                    {
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        CreatedDate = DateTime.Now,
                        Invoices = new List<Invoice>()
                    };

                    var customer2 = new Customer
                    {
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        CreatedDate = DateTime.Now,
                        Invoices = new List<Invoice>()
                    };

                    context.Customers.AddRange(customer1, customer2);
                    context.SaveChanges();
                }

                // Voeg enkele Invoices toe als ze nog niet bestaan
                if (!context.Invoices.Any())
                {
                    var customer1Id = context.Customers.FirstOrDefault(c => c.Name == "John Doe")?.Id ?? 0;
                    var customer2Id = context.Customers.FirstOrDefault(c => c.Name == "Jane Smith")?.Id ?? 0;

                    context.Invoices.AddRange(
                        new Invoice
                        {
                            Amount = 150.00m,
                            InvoiceDate = DateTime.Now.AddMonths(-1),
                            CustomerId = customer1Id
                        },
                        new Invoice
                        {
                            Amount = 200.00m,
                            InvoiceDate = DateTime.Now.AddMonths(-2),
                            CustomerId = customer2Id
                        }
                    );
                    context.SaveChanges();
                }

                // Voeg enkele Payments toe als ze nog niet bestaan
                if (!context.Payments.Any())
                {
                    var invoice1Id = context.Invoices.FirstOrDefault(i => i.Amount == 150.00m)?.Id ?? 0;
                    var invoice2Id = context.Invoices.FirstOrDefault(i => i.Amount == 200.00m)?.Id ?? 0;

                    context.Payments.AddRange(
                        new Payment
                        {
                            Amount = 150.00m,
                            PaymentDate = DateTime.Now.AddMonths(-1),
                            InvoiceId = invoice1Id
                        },
                        new Payment
                        {
                            Amount = 100.00m,
                            PaymentDate = DateTime.Now.AddMonths(-2),
                            InvoiceId = invoice2Id
                        }
                    );
                    context.SaveChanges();
                }
                if (!context.Language.Any())
                {
                    context.Language.AddRange(
                        new Language(),
                        new Language { Code = "en", IsSystemLanguage = true, Name = "English" },
                        new Language { Code = "nl", IsSystemLanguage = true, Name = "Dutch" },
                        new Language { Code = "fr", IsSystemLanguage = true, Name = "French" },
                        new Language { Code = "de", IsSystemLanguage = true, Name = "German" });
                    context.SaveChanges();

                }

                Language.Languages = context.Language.Where(l => l.IsSystemLanguage && l.Code != "?").ToList();
            }
        }
    }
}