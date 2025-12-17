using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive == true);
        }

        public async Task AddAsync(Book book)
        {
            book.CreatedAt = DateTime.Now;
            book.UpdatedAt = DateTime.Now;
            book.CreatedBy = "System";
            book.UpdatedBy = book.CreatedBy;


            _context.Books.Add(book);
            await _context.SaveChangesAsync();
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
