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
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        // Private Fields
        private readonly IGamesGalleryService service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;


        // Public Constructor
        public CategoriesController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }


        // Get : Categories/100/true
        [HttpGet("{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            List<CategoryVM> categories = await service.GetCategoriesAsync(noOfRecords ?? 0, include ?? false);

            if (categories == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
            }
            else if (categories.Count() < 1)
            {
                return NoContent();
            }
            else
            {
                return Ok(categories);
            }
        }


        // Get : Categories/Search/Title/Game/100/true
        [HttpGet("Search/{searchString}/{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> GetSearched([FromRoute] string searchString, [FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                List<CategoryVM> categories = await service.GetSearchedCategoriesAsync(searchString, noOfRecords ?? 0, include ?? false);

                if (categories == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
                }
                else if (categories.Count() < 1)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(categories);
                }
            }

            return BadRequest("Provide a valid search string.");
        }


        // Get : Categories/23512ac0-3253-49a9-8632-5d076bd98e3f/true
        [HttpGet("{id}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool? include)
        {
            CategoryVM category = await service.GetCategoryAsync(id, include ?? false);

            if (category == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Category"));
            }
            else
            {
                return Ok(category);
            }
        }


        // Post : Categories/
        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoryVM model)
        {
            string result = await service.AddCategoryAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnInsert"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Categories" }, $"New Category Added : {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Put : Categories/
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] EditCategoryVM model)
        {
            string result = await service.EditCategoryAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnUpdate"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Categories" }, $"Category Updated : {result}");
            }
            else if (string.IsNullOrEmpty(result))
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Category"));
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Delete : Categories/23512ac0-3253-49a9-8632-5d076bd98e3f
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            string result = await service.DeleteCategoryAsync(id);

            if (result == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404OnDelete"], "Category"));
            }
            else
            {
                return Ok($"Category Deleted : {result}");
            }
        }
    }
}