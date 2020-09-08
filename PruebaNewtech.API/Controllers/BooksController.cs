using Microsoft.AspNetCore.Mvc;
using PruebaNewtech.BOL;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PruebaNewtech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public HttpClient Client { get; }

        public BooksController(HttpClient Client)
        {
            this.Client = Client;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var httpResponse = await Client.GetAsync("/api/books");
            var books = await httpResponse.Content.ReadAsStringAsync();

            return Ok(JsonSerializer.Deserialize<IList<Books>>(books));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpResponse = await Client.GetAsync($"/api/books/{id}");
            var response = await httpResponse.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<Books>(response);

            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Add(Books model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var httpResponse = await Client.PostAsync("/api/books/",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var response = await httpResponse.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<Books>(response);

            if (book == null) return NotFound();

            return Created($"/api/books/{book.ID}", book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Books model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var httpResponse = await Client.PutAsync($"/api/book/{id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var response = await httpResponse.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<Books>(response);

            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var httpResponse = await Client.GetAsync($"/api/books/{id}");
            var response = await httpResponse.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<Books>(response);

            if (book == null) return NotFound();

            await Client.DeleteAsync($"/api/books/{id}");

            return Ok(book);
        }
    }
}
