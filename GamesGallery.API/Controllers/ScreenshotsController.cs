using GamesGallery.API.Services;
using GamesGallery.VM;
using GamesGallery.VM.CreateVM;
using GamesGallery.VM.EditVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesGallery.API.Controllers
{
    [ApiController]
    [Route("screenshots")]
    public class ScreenshotsController : ControllerBase
    {
        // Private Fields
        private readonly IGamesGalleryService service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;


        // Public Constructor
        public ScreenshotsController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }


        // Get : Screenshots/100/true
        [HttpGet("{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            List<ScreenshotVM> screenshots = await service.GetScreenshotsAsync(noOfRecords ?? 0, include ?? false);

            if (screenshots == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
            }
            else if (screenshots.Count() < 1)
            {
                return NoContent();
            }
            else
            {
                return Ok(screenshots);
            }
        }


        // Get : Screenshots/23512ac0-3253-49a9-8632-5d076bd98e3f/true
        [HttpGet("{id}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool? include)
        {
            ScreenshotVM screenshot = await service.GetScreenshotAsync(id, include ?? false);

            if (screenshot == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Screenshot"));
            }
            else
            {
                return Ok(screenshot);
            }
        }


        // Post : Screenshots/
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateScreenshotVM model)
        {
            string result = await service.AddScreenshotAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnInsert"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Screenshots" }, $"New Screenshot Added : {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Put : Screenshots/
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] EditScreenshotVM model)
        {
            string result = await service.EditScreenshotAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnUpdate"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Screenshots" }, $"Screenshot Updated : {result}");
            }
            else if (string.IsNullOrEmpty(result))
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Screenshot"));
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Delete : Screenshots/23512ac0-3253-49a9-8632-5d076bd98e3f
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            string result = await service.DeleteScreenshotAsync(id);

            if (result == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404OnDelete"], "Screenshot"));
            }
            else
            {
                return Ok($"Screenshot Deleted : {result}");
            }
        }
    }
}
