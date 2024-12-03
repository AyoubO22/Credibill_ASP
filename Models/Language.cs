using System.ComponentModel.DataAnnotations;

namespace Credibill_ASP.Models
{
    public class Language
    {
        [Key]
        [StringLength(2)]
        [Display (Name = "Code")]
        public string Code { get; set; } = "en";

    }
}
