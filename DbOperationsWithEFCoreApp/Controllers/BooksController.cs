using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var columnName = "Id";
            var columnValue = "1";

            var parameter = new SqlParameter("columnValue", columnValue);

            var books = await appDbContext.Books
                            .FromSql($"select * from Books where {columnName} = {columnValue}")
                            .ToListAsync();

            var books1 = await appDbContext.Books
                           .FromSqlRaw($"select * from Books where {columnName} = @columnValue", parameter)
                           .ToListAsync();

            return Ok(books);
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            var languages = await appDbContext.Languages.ToListAsync();
            foreach (var language in languages)
            {
                await appDbContext.Entry(language).Collection(x=>x.Books)
                .LoadAsync();
            }
            return Ok(languages);
        }

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

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] int bookId)
        {
            var book = new Book { Id = bookId };
            appDbContext.Entry(book).State = EntityState.Deleted;
            await appDbContext.SaveChangesAsync();

            //var book = await appDbContext.Books.FindAsync(bookId);

            //if (book == null)
            //{
            //    return NotFound();
            //}
            //appDbContext.Books.Remove(book);
            //await appDbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBooksinBulkAsync()
        {
            //var book = new Book { Id = bookId };
            //appDbContext.Entry(book).State = EntityState.Deleted;
            //await appDbContext.SaveChangesAsync();

            var books = await appDbContext.Books.Where(x => x.Id < 8).ExecuteDeleteAsync();

            return Ok();
        }
    }
}
