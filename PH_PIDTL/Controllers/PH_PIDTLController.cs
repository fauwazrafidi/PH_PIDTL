using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
            var item = await _context.PH_PIDTL.OrderBy(p => p.remark2).FirstOrDefaultAsync();
            if (item != null)
            {
                _logger.LogInformation($"ID: {item.id}, REMARK2: {item.remark2}, ITEMCODE: {item.itemcode}, DESCRIPTION: {item.description}, DESCRIPTION2: {item.description2}, BATCH: {item.batch}, LOCATION: {item.location}, QTY: {item.qty}, UOM: {item.uom}");
                return Ok(item);
            }
            else
            {
                _logger.LogWarning("No items found in PH_PIDTLs.");
                return NotFound();
            }

        }

        [HttpGet("items/{searchString}")]
        public async Task<IActionResult> Get([FromQuery] int searchString)  // Use int as the parameter type
        {
            if (searchString == 0) // Check for zero instead of empty
            {
                _logger.LogWarning("Search string is zero.");
                return BadRequest("Search string cannot be zero.");
            }

            var item = await _context.PH_PIDTL
                .Where(p => p.dtlkey == searchString)  // Use direct equality for integer comparison
                .FirstOrDefaultAsync();

            if (item != null)
            {
                _logger.LogInformation($"ID: {item.id}, REMARK2: {item.remark2}, ITEMCODE: {item.itemcode}, DESCRIPTION: {item.description}, DESCRIPTION2: {item.description2}, BATCH: {item.batch}, LOCATION: {item.location}, QTY: {item.qty}, UOM: {item.uom}");
                return Ok(item);
            }
            else
            {
                _logger.LogWarning("No items found with the provided search criteria.");
                return NotFound();
            }
        }

        [HttpGet("export/items/{searchString}")]
        public async Task<IActionResult> Export([FromQuery] int searchString)
        {
            if (searchString == 0)
            {
                _logger.LogWarning("Search string is zero.");
                return BadRequest("Search string cannot be zero.");
            }

            var items = await _context.PH_PIDTL
                .Where(p => p.id == searchString)
                .ToListAsync();

            if (items.Count == 0)
            {
                _logger.LogWarning("No items found with the provided search criteria.");
                return NotFound();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("PH_PIDTL Data");

                // Add header row
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[2, 1].Value = "REMARK2";
                worksheet.Cells[3, 1].Value = "ITEMCODE";
                worksheet.Cells[4, 1].Value = "DESCRIPTION";
                worksheet.Cells[5, 1].Value = "DESCRIPTION2";
                worksheet.Cells[6, 1].Value = "BATCH";
                worksheet.Cells[7, 1].Value = "LOCATION";
                worksheet.Cells[8, 1].Value = "QTY";
                worksheet.Cells[9, 1].Value = "UOM";

                // Add data rows
                int column = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[1, column].Value = item.id;
                    worksheet.Cells[2, column].Value = item.remark2;
                    worksheet.Cells[3, column].Value = item.itemcode;
                    worksheet.Cells[4, column].Value = item.description;
                    worksheet.Cells[5, column].Value = item.description2;
                    worksheet.Cells[6, column].Value = item.batch;
                    worksheet.Cells[7, column].Value = item.location;
                    worksheet.Cells[8, column].Value = item.qty;
                    worksheet.Cells[9, column].Value = item.uom;
                    //column++;
                }

                // Fit columns
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                

                // Set content type and return the file
                var fileName = $"ph_pidtl_{searchString}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return File(memoryStream.ToArray(), contentType, fileName);
                }
            }
        }








    }
}
