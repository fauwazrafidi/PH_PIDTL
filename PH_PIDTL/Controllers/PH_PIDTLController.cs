using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Polynic.Data;


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
            var item = await _context.PH_PIDTL.OrderBy(p => p.id).FirstOrDefaultAsync();
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

        [HttpGet("getItems")]
        public async Task<IActionResult> GetPaginatedItems([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var items = await _context.PH_PIDTL
                .OrderBy(p => p.id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalItems = await _context.PH_PIDTL.CountAsync();

            if (items.Any())
            {
                _logger.LogInformation($"Retrieved {items.Count} items from PH_PIDTL (skip: {skip}, take: {take}).");
                return Ok(items);  // Return the list directly
            }
            else
            {
                _logger.LogWarning("No items found in PH_PIDTLs.");
                return NotFound();
            }
        }

        [HttpGet("item/{Id}")]
        public async Task<IActionResult> Get([FromQuery] int Id)  // Use int as the parameter type
        {
            if (Id == 0)
            {
                _logger.LogWarning("Search string is zero.");
                return BadRequest("Search string cannot be zero.");
            }

            var item = await _context.PH_PIDTL
                .Where(p => p.id == Id)  // Use direct equality for integer comparison
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

        [HttpGet("items/{Remark2}")]
        public async Task<IActionResult> Get([FromQuery] string Remark2)
        {
            if (string.IsNullOrEmpty(Remark2))
            {
                _logger.LogWarning("Search string is empty or null.");
                return BadRequest("Search string cannot be empty or null.");
            }

            // Convert both search string and remark2 to lowercase for case-insensitive comparison
            var normalizedRemark2 = Remark2.ToLower();
            var items = await _context.PH_PIDTL
                .Where(p => p.remark2.ToLower().Contains(normalizedRemark2)) // Use ToLower() for case-insensitive search
                .ToListAsync();

            if (items.Any())
            {
                _logger.LogInformation($"Found {items.Count} items with REMARK2 containing (case-insensitive): {Remark2}");
                return Ok(items);
            }
            else
            {
                _logger.LogWarning("No items found with the provided search criteria.");
                return NotFound();
            }
        }

        [HttpGet("export/item/{Id}")]
        public async Task<IActionResult> Export([FromQuery] int Id, [FromQuery] int amount)
        {
            if (Id == 0)
            {
                _logger.LogWarning("Search string is zero.");
                return BadRequest("Search string cannot be zero.");
            }

            var items = await _context.PH_PIDTL
                .Where(p => p.id == Id)
                .ToListAsync();

            if (items.Count == 0)
            {
                _logger.LogWarning("No items found with the provided search criteria or item has already been checked in.");
                return NotFound("No items found with the provided search criteria or item has already been checked in.");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Generate Excel file content (logic remains the same)
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PH_PIDTL Data");

                // Add header column
                worksheet.Cells[1, 1].Value = "Customer/Vendor";
                worksheet.Cells[2, 1].Value = "Part No";
                worksheet.Cells[3, 1].Value = "Part Name";
                worksheet.Cells[4, 1].Value = "Colour";
                worksheet.Cells[5, 1].Value = "Lot/Batch No";
                worksheet.Cells[6, 1].Value = "Machine No. / Location";
                worksheet.Cells[7, 1].Value = "Quantity // Unit";
                worksheet.Cells[8, 1].Value = "Date Received";
                worksheet.Cells[9, 1].Value = "CheckOut";

                int column = 2;

                worksheet.Cells[1, column].Value = ":";
                worksheet.Cells[2, column].Value = ":";
                worksheet.Cells[3, column].Value = ":";
                worksheet.Cells[4, column].Value = ":";
                worksheet.Cells[5, column].Value = ":";
                worksheet.Cells[6, column].Value = ":";
                worksheet.Cells[7, column].Value = ":";
                worksheet.Cells[8, column].Value = ":";
                worksheet.Cells[9, column].Value = ":";
                

                // Add data rows
                column = 3;
                if(amount == 0)
                {
                    foreach (var item in items)
                    {
                        worksheet.Cells[1, column].Value = item.remark2;
                        worksheet.Cells[2, column].Value = item.itemcode;
                        worksheet.Cells[3, column].Value = item.description;
                        worksheet.Cells[4, column].Value = item.description2;
                        worksheet.Cells[5, column].Value = item.batch;
                        worksheet.Cells[6, column].Value = item.location;
                        worksheet.Cells[7, column].Value = item.qty + "/" + item.qty + item.uom;
                        worksheet.Cells[8, column].Value = item.checkin;
                        worksheet.Cells[9, column].Value = item.checkout;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {

                        worksheet.Cells[1, column].Value = item.remark2;
                        worksheet.Cells[2, column].Value = item.itemcode;
                        worksheet.Cells[3, column].Value = item.description;
                        worksheet.Cells[4, column].Value = item.description2;
                        worksheet.Cells[5, column].Value = item.batch;
                        worksheet.Cells[6, column].Value = item.location;
                        worksheet.Cells[7, column].Value = amount + "/" + item.qty + item.uom;
                        worksheet.Cells[8, column].Value = item.checkin;
                        worksheet.Cells[9, column].Value = item.checkout;
                    }
                }

                

                // Fit columns
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).Width = 1;
                worksheet.Column(3).AutoFit();
                worksheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                // Set content type and return the file
                var fileName = $"ph_pidtl_ID={Id}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return File(memoryStream.ToArray(), contentType, fileName, true);
                }

               
            }

            
        }



        [HttpPut("checkin/id/{Id}")]
        public async Task<IActionResult> CheckIn([FromRoute] int Id)
        {
            if (Id == 0)
            {
                _logger.LogWarning("Item ID cannot be zero.");
                return BadRequest("Item ID cannot be zero.");
            }

            var item = await _context.PH_PIDTL.FindAsync(Id);


            if (item == null)
            {
                _logger.LogWarning($"Item with ID {Id} not found.");
                return NotFound($"Item with ID {Id} not found.");
            }

            if (item.checkin != null)
            {
                _logger.LogWarning($"Item with ID {Id} has already been checked in.");
                return BadRequest("Item has already been checked in.");
            }

            item.checkin = DateTimeOffset.UtcNow.AddHours(8); // Update checkin time

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while updating the checkin time.");
            }

            return Ok("Item checked in successfully.");
        }

        [HttpGet("checkout/id/{Id}")]
        public async Task<IActionResult> GetById([FromQuery] int Id, [FromQuery] int checkoutQty) // Add checkoutQty parameter
        {
            // Find the item with the specified ID
            var item = await _context.PH_PIDTL.FindAsync(Id);

            if (item == null)
            {
                return NotFound("Item with the specified ID not found.");
            }

            if (!item.checkin.HasValue)
            {
                return BadRequest("Item with the specified ID does not have a check-in time.");
            }

            // Validate checkout quantity against qtyremain
            if (checkoutQty > item.qtyremain)
            {
                return BadRequest("Checkout quantity cannot exceed the remaining quantity. Available quantity: " + item.qtyremain);
            }

            // Check if checkout is allowed based on earliest check-in
            var canCheckout = await CanCheckout(item.description, item.description2, item.checkin, Id);

            if (!canCheckout)
            {
                return BadRequest("Checkout not allowed for this item. Another of the same item has an earlier check-in.");
            }

            // Update checkout timestamp and ensure qtyremain stays above 0
            item.checkout = DateTimeOffset.UtcNow.AddHours(8);
            item.qtyremain = Math.Max(item.qtyremain - checkoutQty, 0); // Set qtyremain to 0 if checkout would cause a negative value

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while updating the checkout time and quantity.");
            }

            return Ok(item);
        }

        private async Task<bool> CanCheckout(string description, string description2, DateTimeOffset? checkin, int Id)
        {
            // Find the item with the specified ID (assuming the method is called from GetById)
            var item = await _context.PH_PIDTL.FindAsync(Id); // Assuming Id is accessible here

            // Store the ID in a local variable
            int itemId = item.id;

            // Find items with the same description and description2
            var items = await _context.PH_PIDTL
                .Where(p => p.description == description && p.description2 == description2)
                .ToListAsync();

            // Check if any item has an earlier check-in than the current item
            return !items.Any(i => i.id != itemId && i.checkin < checkin);
        }



    }
}
