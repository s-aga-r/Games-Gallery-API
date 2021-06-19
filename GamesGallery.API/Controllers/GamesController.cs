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
    [Route("games")]
    public class GamesController : ControllerBase
    {
        // Private Fields
        private readonly IGamesGalleryService service;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;


        // Public Constructor
        public GamesController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
            this.configuration = configuration;
        }


        // Get : Games/100/true
        [HttpGet("{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            List<GameVM> games = await service.GetGamesAsync(noOfRecords ?? 0, include ?? false);

            if (games == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
            }
            else if (games.Count() < 1)
            {
                return NoContent();
            }
            else
            {
                return Ok(games);
            }
        }


        // Get : Games/Sort/Title/Desc/100/true
        [HttpGet("Sort/{sortBy}/{orderBy}/{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> GetSorted([FromRoute] string sortBy, [FromRoute] string orderBy, [FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(orderBy))
            {
                sortBy = sortBy.ToUpper();
                orderBy = orderBy.ToUpper();

                List<string> allowedSortByTypes = new List<string>() { "TITLE", "SIZE", "TOTALDOWNLOADS", "YEAROFRELEASE", "LASTUPDATEDON" };
                List<string> allowedOrderByTypes = new List<string>() { "ASC", "DESC" };

                if (allowedSortByTypes.Contains(sortBy) && (allowedOrderByTypes.Contains(orderBy)))
                {
                    List<GameVM> games = await service.GetSortedGamesAsync(sortBy, orderBy, noOfRecords ?? 0, include ?? false);

                    if (games == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
                    }
                    else if (games.Count() < 1)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(games);
                    }
                }
            }

            return BadRequest("Supported sortBy types are : Title, Size, TotalDownloads, YearOfRelease, LastUpdatedOn. \nSupported orderBy types are : ASC, DESC.");
        }


        // Get : Games/Search/Title/Game/100/true
        [HttpGet("Search/{searchBy}/{searchString}/{noOfRecords:int}/{include:bool}")]
        public async Task<IActionResult> GetSearched([FromRoute] string searchBy, [FromRoute] string searchString, [FromRoute] int? noOfRecords, [FromRoute] bool? include)
        {
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchString))
            {
                searchBy = searchBy.ToUpper();
                searchString = searchString.ToUpper();

                List<string> allowedSearchByTypes = new List<string>() { "TITLE", "SIZE", "TOTALDOWNLOADS", "DATEOFUPLOAD", "YEAROFRELEASE", "LASTUPDATEDON" };

                if (allowedSearchByTypes.Contains(searchBy))
                {
                    List<GameVM> games = await service.GetSearchedGamesAsync(searchBy, searchString, noOfRecords ?? 0, include ?? false);

                    if (games == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"]);
                    }
                    else if (games.Count() < 1)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok(games);
                    }
                }
            }

            return BadRequest("Supported searchBy types are : Title, Size, TotalDownloads, DateOfUpload, YearOfRelease, LastUpdatedOn.");
        }


        // Get : Games/23512ac0-3253-49a9-8632-5d076bd98e3f/true
        [HttpGet("{id}/{include:bool}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] bool? include)
        {
            GameVM game = await service.GetGameAsync(id, include ?? false);

            if (game == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Game"));
            }
            else
            {
                return Ok(game);
            }
        }


        // Post : Games/
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateGameVM model)
        {
            string result = await service.AddGameAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnInsert"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Games" }, $"New Game Added : {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Put : Games/
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] EditGameVM model)
        {
            string result = await service.EditGameAsync(model);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500OnUpdate"]);
            }
            else if (result.Length == 36)
            {
                return CreatedAtAction(nameof(Get), new { id = result, include = true, controller = "Games" }, $"Game Updated : {result}");
            }
            else if (string.IsNullOrEmpty(result))
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404"], "Game"));
            }
            else
            {
                return BadRequest(result);
            }
        }


        // Delete : Games/23512ac0-3253-49a9-8632-5d076bd98e3f
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            string result = await service.DeleteGameAsync(id);

            if (result == null)
            {
                return NotFound(string.Format(configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode404OnDelete"], "Game"));
            }
            else
            {
                return Ok($"Game Deleted : {result}");
            }
        }
    }
}