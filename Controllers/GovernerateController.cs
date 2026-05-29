using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel;
using Tourism_Api.Entity.Governorate;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableRateLimiting(RateLimiters.Concurrency)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public class GovernerateController(IGovernorateService governorateService) : ControllerBase
    {
        private readonly IGovernorateService _governorateService = governorateService;

        [HttpGet("All-Governorate-Pagnation")]
        [ProducesResponseType(typeof(PaginatedList<GovernorateResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGovernoratePgnation([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernoratePagnation(requestFilters, cancellationToken);
            //return Ok(result.Value);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpGet("All-Governorate")]
        [ProducesResponseType(typeof(Governorates), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGovernorate(CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorate(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("GovernorateAndPlaces-pagnation")]
        [ProducesResponseType(typeof(PaginatedList<GovernorateALLPlaces>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGovernorateAndPlacesPagnation([FromQuery] RequestFiltersScpical requestFilters, CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorateAndPlacesAsync(requestFilters, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("GovernorateAndPlaces")]
        [ProducesResponseType(typeof(GovernorateAndPLacesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGovernorateAndPlaces(string name, CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorateAndPlace(name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("All-GovernoratesName")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGovernoratesName(CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernoratesName(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("SearchForGovernorate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ALLPGeneratorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchForGovernorate([FromQuery] string name, CancellationToken cancellationToken)
        {
            var result = await _governorateService.SearchForGovernorate(name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [AllowAnonymous]
        // upload exal sheet 
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (!file.FileName.EndsWith(".xlsx"))
                return BadRequest("Invalid file format. Please upload an Excel file (.xlsx).");


            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // الورقة الأولى
                    var rowCount = worksheet.Dimension.Rows;

                    var items = new List<CityInfo>();

                    // افتراض أن الصف الأول يحتوي على رأس (Header)
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var item = new CityInfo
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString(),
                            Photo = worksheet.Cells[row, 2].Value?.ToString()
                        };

                        // التحقق من صحة البيانات
                        if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Photo))
                        {
                            // اختياري: التحقق من أن الرابط صالح
                            if (Uri.TryCreate(item.Photo, UriKind.Absolute, out _))
                            {
                                items.Add(item);
                            }
                        }
                    }

                    // إضافة البيانات إلى قاعدة البيانات
                    if (items.Any())
                    {
                        //await _context.Items.AddRangeAsync(items);
                        //await _context.SaveChangesAsync();
                        return Ok(items);
                    }
                    else
                    {
                        return BadRequest("No valid data found in the Excel file.");
                    }
                }
            }


        }









    }

}
  
 
