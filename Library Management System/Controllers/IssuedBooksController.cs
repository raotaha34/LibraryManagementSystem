using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    public class IssuedBooksController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IBookServices _BookService;
        private readonly IMemberService _MemberService;


        public IssuedBooksController(IIssueService issueService, IBookServices bookService, IMemberService memberService)
        {
            _issueService = issueService;
            _BookService = bookService;
            _MemberService = memberService;

        }
        public async Task<IActionResult> Index()
        {
            var issuedBooks = await _issueService.GetAllIssuedBooksAsync();
            return View(issuedBooks);
        }
        
        
        
        public async Task<IActionResult> Issue()
        {
            var  books = await _BookService.GetAllAsync();
            var members = await _MemberService.GetAllAsync();

            var BookMemberModel = new BookMemberModel
            {
                books = books,
                members = members
            };

            return View(BookMemberModel);
        }

        [HttpPost]
        public async Task<IActionResult> Issue(int bookId, int memberId)
        {
            var success = await _issueService.IssueBookAsync(bookId, memberId);

            if (success == false)
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
