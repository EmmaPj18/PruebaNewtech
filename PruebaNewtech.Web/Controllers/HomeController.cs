using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaNewtech.BOL;
using PruebaNewtech.Web.Models;

namespace PruebaNewtech.Web.Controllers
{
    public class HomeController : Controller
    {
        public HttpClient Client { get; set; }

        public HomeController(HttpClient Client)
        {
            this.Client = Client;
        }

        public async Task<IActionResult> Index()
        {
            var httpResponse = await Client.GetAsync("books/");
            var result = await httpResponse.Content.ReadAsStringAsync();

            var books = JsonSerializer.Deserialize<IList<Books>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
