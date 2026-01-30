
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class MemberDashboardController : Controller
    {
        private readonly IBookServices _bookService;
        private readonly IIssueService _issueService;

        public MemberDashboardController(
            IBookServices bookService,
            IIssueService issueService)
        {
            _bookService = bookService;
            _issueService = issueService;
        }

        public IActionResult Index()
        {
            int? memberId = HttpContext.Session.GetInt32("UserId");
            string role = HttpContext.Session.GetString("UserRole") ?? "";

            if (memberId == null || role != "Member")
                return RedirectToAction("Index", "Home"); 

            return View("Index","MemberDashboard"); 
        }

        // View all books page
        public async Task<IActionResult> ViewBooks()
        {
            int? memberId = HttpContext.Session.GetInt32("UserId");
            string role = HttpContext.Session.GetString("UserRole") ?? "";

            if (memberId == null || role != "Member")
                return RedirectToAction("Index", "Home");

            var books = await _bookService.GetAllAsync();
            ViewBag.Books = books;

            return View("AvailableBooks");
        }

        // Return issued books page
        public async Task<IActionResult> ReturnBooks()
        {
            int? memberId = HttpContext.Session.GetInt32("UserId");
            string role = HttpContext.Session.GetString("UserRole") ?? "";

            if (memberId == null || role != "Member")
                return RedirectToAction("Index", "Home");

            var issuedBooks = await _issueService.GetIssuedBooksByMemberAsync(memberId.Value);
            ViewBag.IssuedBooks = issuedBooks;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _issueService.ReturnBookAsync(id);
            return RedirectToAction("ReturnBooks");
        }

    
    }
}