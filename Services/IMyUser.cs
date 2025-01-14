using System.Linq;
using Credibill_ASP.Data;
using CrediBill_ASP.Data;
using Credibill_ASP.Models;
using CrediBill_ASP.Models;
using Microsoft.AspNetCore.Http;

namespace CrediBill_ASP.Services
{
    public interface IMyUser
    {
        public CredibillUser User { get; }
    }

    public class MyUser : IMyUser
    {
        AppDbContext _context;
        IHttpContextAccessor _httpContext;

        public CredibillUser User { get { return GetUser(); } }


        public MyUser(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public CredibillUser GetUser()
        {
            string name = _httpContext.HttpContext.User.Identity.Name;
            if (name == null || name == "")
                return Globals.DefaultUser;
            else
                return _context.Users.FirstOrDefault(u => u.UserName == name);
        }
    }
}