using Library_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Library_Management_System.Services.Interfaces;
namespace Library_Management_System.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookServices _bookService;

        public BooksController(IBookServices bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _bookService.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            await _bookService.AddAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            await _bookService.UpdateAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
