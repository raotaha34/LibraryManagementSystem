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

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var books = await _bookService.GetAllPaginatedAsync(page, pageSize);
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            var isAdded =  await _bookService.AddAsync(book);

            if(isAdded == false)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "This book already exists.");
                    return View(book);
                }
            }
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
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
