using System;
using System.Collections.Generic;
using CrediBill_ASP.Data;
using CrediBill_ASP.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Credibill_ASP.Models;


namespace GroupBudget_Web.ViewModels
{
    public class UserViewModel
    {
        [Key]
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }  // combined first + last name

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }

        [Display(Name = "Blocked ?")]
        public Boolean IsBlocked { get; set; } = false;


        public UserViewModel()
        {
        }

        public UserViewModel(CredibillUser user, AppDbContext context)
        {
            UserId = user.Id;
            UserName = user.UserName;
            UserEmail = user.Email;
            Name = user.ToString();
            IsBlocked = user.LockoutEnd > DateTime.Now;
            Roles = (from userRole in context.UserRoles
                    where userRole.UserId == user.Id
                    select userRole.RoleId)
                .ToList();
        }
    }
}