using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class IssuedBooksController : Controller
    {
        private readonly IIssueService _issueService;

        public IssuedBooksController(IIssueService issueService)
        {
            _issueService = issueService;
        }
        public async Task<IActionResult> Index()
        {
            var issuedBooks = await _issueService.GetAllIssuedBooksAsync();
            return View(issuedBooks);
        }
        public IActionResult Issue()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Issue(int bookId, int memberId)
        {
            var success = await _issueService.IssueBookAsync(bookId, memberId);

            if (!success)
            {
                ModelState.AddModelError("", "Book not available or invalid member.");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // Return book
        public async Task<IActionResult> Return(int id)
        {
            var success = await _issueService.ReturnBookAsync(id);

            if (!success)
            {
                TempData["Error"] = "Unable to return book. Invalid issued book.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
