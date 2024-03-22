using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polynic.Data;

namespace Polynic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PH_PIDTLController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PH_PIDTLController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _context.PH_PIDTLs.ToListAsync();
            foreach (var item in items)
            {
                Console.WriteLine($"REMARK2: {item.REMARK2}, ITEMCODE: {item.ITEMCODE}, DESCRIPTION: {item.DESCRIPTION}, DESCRIPTION2: {item.DESCRIPTION2}, BATCH: {item.BATCH}, LOCATION: {item.LOCATION}, QTY: {item.QTY}, UOM: {item.UOM}");
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.PH_PIDTLs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


    }
}
