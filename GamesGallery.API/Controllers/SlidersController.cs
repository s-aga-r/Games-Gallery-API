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
    [Route("sliders")]
    public class SlidersController : ControllerBase
    {
        // Private Fields
        private readonly IGamesGalleryService service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;


        // Public Constructor
        public SlidersController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }


        // Get : Sliders/100/true
        [HttpGet("{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            List<SliderVM> sliders = await service.GetSlidersAsync(noOfRecords ?? 0, include ?? false);

            if (sliders == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
            }
            else if (sliders.Count() < 1)
            {
                return NoContent();
            }
            else
            {
                return Ok(sliders);
            }
        }


        // Get : Sliders/23512ac0-3253-49a9-8632-5d076bd98e3f/true
        [HttpGet("{id}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool? include)
        {
            SliderVM slider = await service.GetSliderAsync(id, include ?? false);

            if (slider == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Slider"));
            }
            else
            {
                return Ok(slider);
            }
        }


        // Post : Sliders/
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateSliderVM model)
        {
            string result = await service.AddSliderAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnInsert"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Sliders" }, $"New Slider Added : {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Put : Sliders/
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] EditSliderVM model)
        {
            string result = await service.EditSliderAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnUpdate"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Sliders" }, $"Slider Updated : {result}");
            }
            else if (string.IsNullOrEmpty(result))
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Slider"));
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Delete : Sliders/23512ac0-3253-49a9-8632-5d076bd98e3f
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            string result = await service.DeleteSliderAsync(id);

            if (result == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404OnDelete"], "Slider"));
            }
            else
            {
                return Ok($"Slider Deleted : {result}");
            }
        }
    }
}
