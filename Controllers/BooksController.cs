using LibraryApp.Hubs;
using LibraryApp.Models;
using LibraryApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookRepository _repo;

        public BooksController(IBookRepository repo)
        {
            _repo = repo;
        }

        [ResponseCache(Duration = 30)]
        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            _repo.Add(book);
            TempData["Success"] = $"\"{book.Title}\" added!";

            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<BookHub>>();
            await hub.Clients.All.SendAsync("BookAdded");

            return RedirectToAction(nameof(Index));
        }
    }
}