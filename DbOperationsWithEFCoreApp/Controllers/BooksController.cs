using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
