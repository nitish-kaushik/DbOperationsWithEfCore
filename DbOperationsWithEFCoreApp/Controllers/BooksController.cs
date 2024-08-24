using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {
            appDbContext.Books.Add(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> model)
        {
            appDbContext.Books.AddRange(model);
            await appDbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] Book model)
        {
            var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = model.Title;
            book.Description = model.Description;
            book.NoOfPages = model.NoOfPages;

            await appDbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleQuery([FromBody] Book model)
        {
            appDbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await appDbContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulk()
        {
            await appDbContext.Books
                .Where(x => x.NoOfPages == 100)
                .ExecuteUpdateAsync(x => x
            .SetProperty(p => p.Description, p => p.Title + " This is book description 2")
            .SetProperty(p => p.Title, p => p.Title + " updated 2")
            //.SetProperty(p => p.NoOfPages, 100)
            );
            return Ok();
        }
    }
}
