using LibraryApp.Data;
using LibraryApp.Models;
using System.Collections.Generic;

namespace LibraryApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll() => _context.Books.ToList();

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
    }
}