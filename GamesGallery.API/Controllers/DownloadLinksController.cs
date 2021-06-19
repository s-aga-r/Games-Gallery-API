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
    [Route("download-links")]
    public class DownloadLinksController : Controller
    {
        // Private Fields
        private readonly IGamesGalleryService service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;


        // Public Constructor
        public DownloadLinksController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }


        // Get : DownloadLinks/100/true
        [HttpGet("{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            List<DownloadLinkVM> downloadLinks = await service.GetDownloadLinksAsync(noOfRecords ?? 0, include ?? false);

            if (downloadLinks == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
            }
            else if (downloadLinks.Count() < 1)
            {
                return NoContent();
            }
            else
            {
                return Ok(downloadLinks);
            }
        }


        // Get : DownloadLinks/Search/Title/Link1/100/true
        [HttpGet("Search/{searchBy}/{searchString}/{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> GetSearched([FromRoute] string searchBy, [FromRoute] string searchString, [FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchString))
            {
                searchBy = searchBy.ToUpper();
                searchString = searchString.ToUpper();

                List<string> allowedSearchByTypes = new List<string>() { "TITLE", "LINK" };

                if (allowedSearchByTypes.Contains(searchBy))
                {
                    List<DownloadLinkVM> downloadLinks = await service.GetSearchedDownloadLinksAsync(searchBy, searchString, noOfRecords ?? 0, include ?? false);

                    if (downloadLinks == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
                    }
                    else if (downloadLinks.Count() < 1)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(downloadLinks);
                    }
                }
            }

            return BadRequest("Supported searchBy types are : Title and Link.");
        }


        // Get : DownloadLinks/23512ac0-3253-49a9-8632-5d076bd98e3f/true
        [HttpGet("{id}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool? include)
        {
            DownloadLinkVM downloadLink = await service.GetDownloadLinkAsync(id, include ?? false);

            if (downloadLink == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Download Link"));
            }
            else
            {
                return Ok(downloadLink);
            }
        }


        // Post : DownloadLinks/
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateDownloadLinkVM model)
        {
            string result = await service.AddDownloadLinkAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnInsert"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "DownloadLinks" }, $"New Download Link Added : {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Put : DownloadLinks/
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] EditDownloadLinkVM model)
        {
            string result = await service.EditDownloadLinkAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnUpdate"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "DownloadLinks" }, $"Download Link Updated : {result}");
            }
            else if (string.IsNullOrEmpty(result))
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Download Link"));
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Delete : DownloadLinks/23512ac0-3253-49a9-8632-5d076bd98e3f
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            string result = await service.DeleteDownloadLinkAsync(id);

            if (result == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404OnDelete"], "Download Link"));
            }
            else
            {
                return Ok($"Download Link Deleted : {result}");
            }
        }
    }
}
