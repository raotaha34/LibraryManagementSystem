using Library_Management_System.Common;
using Library_Management_System.Models;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Library_Management_System.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Library_Management_System.Services
{
    public class IssueService : IIssueService
    {
        private readonly LibraryManagementContext _context;

        public IssueService(LibraryManagementContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<IssuedBook>> GetAllIssuedBooksAsync(int page, int pageSize)
        {
            var query =   _context.IssuedBooks
                .Include(i => i.Member)
                .Where(i => i.IsActive == true && i.ReturnDate == null).OrderByDescending(x => x.IssueDate);

            return await PaginatedHelper<IssuedBook>.CreateAsync(query, page, pageSize);
        }
        

        public async Task<bool> IssueBookAsync(int bookId, int memberId)
        {
            var alreadyExist = await _context.IssuedBooks.FirstOrDefaultAsync(i => i.BookId == bookId && i.MemberId == memberId && i.ReturnDate == null);
            if (alreadyExist != null)
            {
                return false;
            }
            var book = _context.Books.Where(x => x.Id == bookId).FirstOrDefault();
            if (book == null || book.Quantity <= 0)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                book.Quantity--;

                var issuedBook = new IssuedBook
                {
                    BookId = bookId,
                    MemberId = memberId,
                    IssueDate = DateTime.Now,
                    ReturnDate = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true
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
            var issuedBook = _context.IssuedBooks
                .Where(x => x.Id == issuedBookId).FirstOrDefault();

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
