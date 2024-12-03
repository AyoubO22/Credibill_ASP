using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Credibill_ASP.Models
{
    public class Language
    {
        [Key]
        [StringLength(2)]
        [Display (Name = "Code")]
        public string Code { get; set; } = "?";

        [Required]
        [Display(Name = "Language")]
        public string Name { get; set; } = "?";

        [Required]
        public bool IsSystemLanguage { get; set; } = false;

        static public List<Language>Languages { get; set; }

    }
}
