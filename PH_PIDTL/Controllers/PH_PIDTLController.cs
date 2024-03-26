using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polynic.Data;
using Polynic.Models;

namespace Polynic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PH_PIDTLController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PH_PIDTLController> _logger;

        public PH_PIDTLController(ApplicationDbContext context, ILogger<PH_PIDTLController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var item = await _context.PH_PIDTL.OrderBy(p => p.REMARK2).FirstOrDefaultAsync();
            if (item != null)
            {
                _logger.LogInformation($"REMARK2: {item.REMARK2}, ITEMCODE: {item.ITEMCODE}, DESCRIPTION: {item.DESCRIPTION}, DESCRIPTION2: {item.DESCRIPTION2}, BATCH: {item.BATCH}, LOCATION: {item.LOCATION}, QTY: {item.QTY}, UOM: {item.UOM}");
                return Ok(item);
            }
            else
            {
                _logger.LogWarning("No items found in PH_PIDTLs.");
                return NotFound();
            }

        }

        [HttpGet("items/{searchString}")]
        public async Task<IActionResult> Get([FromQuery] string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                _logger.LogWarning("Search string is empty.");
                return BadRequest("Search string cannot be empty.");
            }

            var item = await _context.PH_PIDTL
                .Where(p => EF.Functions.Like(p.REMARK2, $"%{searchString}%"))
                .FirstOrDefaultAsync();

            if (item != null)
            {
                _logger.LogInformation($"REMARK2: {item.REMARK2}, ITEMCODE: {item.ITEMCODE}, DESCRIPTION: {item.DESCRIPTION}, DESCRIPTION2: {item.DESCRIPTION2}, BATCH: {item.BATCH}, LOCATION: {item.LOCATION}, QTY: {item.QTY}, UOM: {item.UOM}");
                return Ok(item);
            }
            else
            {
                _logger.LogWarning("No items found with the provided search criteria.");
                return NotFound();
            }
        }







    }
}
