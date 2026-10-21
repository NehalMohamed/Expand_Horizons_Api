using ITravel_App.Services.ExcelService;
using Microsoft.AspNetCore.Mvc;

namespace ITravel_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripImportController : ControllerBase
    {
        private readonly TripExcelImportService _excelService;
        private readonly TripImportRepository _repository;

        public TripImportController(
            TripExcelImportService excelService,
            TripImportRepository repository)
        {
            _excelService = excelService;
            _repository = repository;
        }

        [HttpPost("import")]
        [RequestSizeLimit(20 * 1024 * 1024)]
        public async Task<IActionResult> Import(
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Please upload an Excel file."
                });
            }

            var extension =
                Path.GetExtension(file.FileName)
                    .ToLowerInvariant();

            if (extension != ".xlsx")
            {
                return BadRequest(new
                {
                    message = "Only .xlsx files are supported."
                });
            }

            try
            {
                await using var stream =
                    file.OpenReadStream();

                var (trips, errors) =
                    await _excelService.ReadExcelAsync(stream);

                // Validation errors
                if (errors.Count > 0)
                {
                    return BadRequest(new
                    {
                        message = "Excel validation failed.",
                        errors
                    });
                }

                if (trips.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "No trips were found in the Excel file."
                    });
                }

               // await _repository.ImportTripsAsync(trips);

                return Ok(new
                {
                    message = "Trips imported successfully.",
                    imported = trips.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An error occurred while importing the Excel file.",
                        error = ex.Message
                    });
            }
        }
    }
    }
