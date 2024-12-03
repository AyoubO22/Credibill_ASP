using System;
using System.ComponentModel.DataAnnotations;

namespace CrediBill_ASP.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        // Relatie
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}