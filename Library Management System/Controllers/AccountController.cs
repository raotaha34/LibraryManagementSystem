using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Validate user using your existing service
            var user = _accountService.ValidateUser(email, password);

            if (user == null)
            {
                // Login failed: stay on login page and show error
                ViewBag.LoginError = "Invalid email or password";
                return View("Index"); // show login page again
            }

            // Login successful: store session values
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Redirect based on role
            if (user.Role == "Member")
                return RedirectToAction("Index", "MemberDashboard"); // member goes to dashboard

            if (user.Role == "Admin")
                return RedirectToAction("Index", "Home"); // admin goes to home/admin dashboard

            // fallback
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    

    }

}
