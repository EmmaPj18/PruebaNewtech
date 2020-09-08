using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PruebaNewtech.BOL;
using PruebaNewtech.Web.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using X.PagedList;

namespace PruebaNewtech.Web.Controllers
{
    public class HomeController : Controller
    {
        public HttpClient Client { get; set; }
        public int PageSize { get; set; }

        public HomeController(HttpClient Client, IConfiguration configuration)
        {
            this.Client = Client;
            PageSize = int.Parse(configuration["PageSize"]);
        }

        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var httpResponse = await Client.GetAsync("books/");
            var result = await httpResponse.Content.ReadAsStringAsync();

            var books = JsonSerializer.Deserialize<IList<Books>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (!string.IsNullOrEmpty(searchString))
                books = books.Where(s => s.Description.Contains(searchString) || s.Title.Contains(searchString) || s.ID.ToString().Contains(searchString))
                    .ToList();

            int pageNumber = (page ?? 1);

            return View(books.ToPagedList(pageNumber, PageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpResponse = await Client.GetAsync($"books/{id}");
            var result = await httpResponse.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<Books>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (book == null) return View("Index");

            var authorHttpResponse = await Client.GetAsync($"authors/books/{book.ID}");
            var authorResult = await authorHttpResponse.Content.ReadAsStringAsync();

            var authors = JsonSerializer.Deserialize<IList<Authors>>(authorResult, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (authors == null) return View("Index");

            return View(new BooksAuthorViewModel { Book = book, Author = authors });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
