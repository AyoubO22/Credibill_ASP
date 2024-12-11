using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Credibill_ASP.Models
{
    public class CredibillUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(2)]
        [ForeignKey("Languages")]
        public string Languagecode { get; set; } = "?";

        public override string ToString()
        {
            return FirstName + " " + LastName; 
        }
    }
}
