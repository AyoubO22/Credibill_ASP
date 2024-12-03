using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Credibill_ASP.Models;
using CrediBill_ASP.Models;

namespace Credibill_ASP.Models
{
    public class Customer
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? Deleted { get; set; } = DateTime.MaxValue;

    // Relatie
    public required ICollection<Invoice> Invoices { get; set; }
}
}


  


