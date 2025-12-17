using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class IssueService : IIssueService
    {
        private readonly LibraryManagementContext _context;

        public IssueService(LibraryManagementContext context)
        {
            _context = context;
        }
        public async Task<List<IssuedBook>> GetAllIssuedBooksAsync()
        {
            return await _context.IssuedBooks
                .Include(i => i.Member)
                .Where(i => i.IsActive == true)
                .ToListAsync();
        }
        // public async Task<List<IssuedBook>> 

        public async Task<bool> IssueBookAsync(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.Quantity <= 0)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                book.Quantity--;

                var issuedBook = new IssuedBook
                {
                    BookId = bookId,
                    MemberId = memberId
                };

                _context.IssuedBooks.Add(issuedBook);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }

        public async Task<bool> ReturnBookAsync(int issuedBookId)
        {
            var issuedBook = await _context.IssuedBooks
                .FirstOrDefaultAsync(i => i.Id == issuedBookId);

            if (issuedBook == null) return false;
            var Book = await _context.Books.FindAsync(issuedBook.BookId);

            if (issuedBook == null)
                return false;

            issuedBook.ReturnDate = DateTime.Now;
            if (Book != null)
            {
                Book.Quantity++;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
