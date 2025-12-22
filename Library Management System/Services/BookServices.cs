using Library_Management_System.Common;
using Library_Management_System.Helpers;
using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Library_Management_System.Services
{
    public class BookService : IBookServices
    {
        private readonly LibraryManagementContext _context;

        public BookService(LibraryManagementContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Where(b => b.IsActive == true)
                .ToListAsync();
        }
        public async Task<PaginatedList<Book>> GetAllPaginatedAsync(int page, int pageSize)
        {
            var query =  _context.Books
                .Where(b => b.IsActive == true);

            return await PaginatedHelper<Book>.CreateAsync(query, page, pageSize);
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive == true);
        }

        public async Task<bool> AddAsync(Book book)
        {
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == book.Title && b.Author == book.Author && b.IsActive == true);
            var softdeletedBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title == book.Title && b.Author == book.Author && b.IsActive == false);
            if (existingBook != null)
            {
                return false;
            }
            else if (softdeletedBook != null)
            {
                softdeletedBook.IsActive = true;
                _context.Books.Update(softdeletedBook);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                book.CreatedAt = DateTime.Now;
                book.UpdatedAt = DateTime.Now;
                book.CreatedBy = "System";
                book.UpdatedBy = book.CreatedBy;
                book.IsActive = true;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task UpdateAsync(Book book)
        {
            book.CreatedAt = DateTime.Now;
            book.UpdatedAt = DateTime.Now;
            book.CreatedBy = "System";
            book.UpdatedBy = book.CreatedBy;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book == null) return;

            book.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
