using LibraryApp.Models;
using System.Collections.Generic;

namespace LibraryApp.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        void Add(Book book);
        // We don't need the others for this assessment – they are optional
    }
}