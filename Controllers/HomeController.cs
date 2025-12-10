using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            
            // Change the URL to match your running app (check the browser address bar)
            var response = await client.GetStringAsync("https://localhost:7234/api/books");
            
            // Optional: show raw JSON on the page
            ViewBag.ApiResult = response;

            return View();
        }
    }
}