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
    public class AuthorsController : ControllerBase
    {
        public HttpClient Client { get; }

        public AuthorsController(HttpClient Client)
        {
            this.Client = Client;
        }

        [HttpGet("books/{idBook}")]
        public async Task<IActionResult> GetByBook(int idBook)
        {
            var httpResponse = await Client.GetAsync($"/api/authors/books/{idBook}");
            var response = await httpResponse.Content.ReadAsStringAsync();

            var author = JsonSerializer.Deserialize<Authors>(response);

            if (author == null) return NotFound();

            return Ok(author);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var httpResponse = await Client.GetAsync("/api/authors");
            var books = await httpResponse.Content.ReadAsStringAsync();

            return Ok(JsonSerializer.Deserialize<IList<Authors>>(books));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var httpResponse = await Client.GetAsync($"/api/authors/{id}");
            var response = await httpResponse.Content.ReadAsStringAsync();

            var author = JsonSerializer.Deserialize<Authors>(response);

            if (author == null) return NotFound();

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Authors model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var httpResponse = await Client.PostAsync("/api/authors/",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var response = await httpResponse.Content.ReadAsStringAsync();

            var author = JsonSerializer.Deserialize<Authors>(response);

            if (author == null) return NotFound();

            return Created($"/api/authors/{author.ID}", author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Authors model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var httpResponse = await Client.PutAsync($"/api/authors/{id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var response = await httpResponse.Content.ReadAsStringAsync();

            var author = JsonSerializer.Deserialize<Authors>(response);

            if (author == null) return NotFound();

            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var httpResponse = await Client.GetAsync($"/api/authors/{id}");
            var response = await httpResponse.Content.ReadAsStringAsync();

            var author = JsonSerializer.Deserialize<Books>(response);

            if (author == null) return NotFound();

            await Client.DeleteAsync($"/api/authors/{id}");

            return Ok(author);
        }
    }
}
