using CrediBill_ASP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Credibill_ASP.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

        // Relatie
        public ICollection<Invoice> Invoices { get; set; }
    }
}