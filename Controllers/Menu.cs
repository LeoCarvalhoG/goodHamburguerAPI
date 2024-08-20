// Controllers/Menu.cs
using GoodHamburgerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Menu : ControllerBase
    {
        private readonly AppDbContext _context;

        public Menu(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("sandwiches")]
        public async Task<IActionResult> GetSandwiches()
        {
            var sandwiches = await _context.Sandwiches.ToListAsync();
            return Ok(sandwiches);
        }

        [HttpGet("extras")]
        public async Task<IActionResult> GetExtras()
        {
            var extras = await _context.Extras.ToListAsync();
            return Ok(extras);
        }

        [HttpGet("sandwiches/{id}")]
        public async Task<IActionResult> GetSandwich(int id)
        {
            var sandwich = await _context.Sandwiches.FindAsync(id);
            if (sandwich == null)
                return NotFound();
            return Ok(sandwich);
        }

        [HttpGet("extras/{id}")]
        public async Task<IActionResult> GetExtra(int id)
        {
            var extra = await _context.Extras.FindAsync(id);
            if (extra == null)
                return NotFound();
            return Ok(extra);
        }
    }
}
