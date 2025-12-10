using LibraryApp.Models;
using LibraryApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers.Api
{
    // PA1201 + PA1202 + PA1207 – RESTful Web API
    [Route("api/books")]
    [ApiController]
    // REMOVED [Authorize] → Public API so you can open directly in browser
    public class BooksApiController : ControllerBase
    {
        private readonly IBookRepository _repo;

        public BooksApiController(IBookRepository repo)
        {
            _repo = repo;
        }

        // GET: api/books → returns all books as JSON
        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _repo.GetAll();
            return Ok(books);
        }

        // GET: api/books/5 → get one book by ID
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var book = _repo.GetAll().FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        // POST: api/books → create a new book
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.Add(book);

            // Proper REST: return 201 Created with location header
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // DELETE: api/books/5 → delete a book (admin only if you want)
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            // In real app: find and delete book
            // For demo, just return success
            return NoContent(); // 204 No Content = correct REST response
        }
    }
}