using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Credibill_ASP.Models;

namespace CrediBill_ASP.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        // Relaties
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}