using ListingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListingApp.Controllers
{
    public class BaseController : Controller
    {
        protected bool EnsureAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");

            return role == Role.Admin.ToString();
        }
    }
}
