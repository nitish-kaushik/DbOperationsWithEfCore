using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            //var result = _appDbContext.Currencies.ToList();
            //var result = (from currencies in _appDbContext.Currencies
            //select currencies).ToList();

            //var result = await _appDbContext.Currencies.ToListAsync();
            var result = await (from currencies in _appDbContext.Currencies
                                select currencies).ToListAsync();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        {
            var result = await _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {
            //var result = await _appDbContext.Currencies
            //    .FirstOrDefaultAsync(x => 
            //    x.Title == name 
            //    && (string.IsNullOrEmpty(description) || x.Description == description)
            //    );

            var result = await _appDbContext.Currencies
                .Where(x =>
                x.Title == name
                && (string.IsNullOrEmpty(description) || x.Description == description)
                ).ToListAsync();

            return Ok(result);
        }
    }
}
