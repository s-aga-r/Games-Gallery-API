using GamesGallery.API.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GamesGallery.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        // Private Fields
        private readonly INLogLogger _logger;
        private readonly IConfiguration _configuration;


        // Public Constructor
        public ErrorController(INLogLogger logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }


        [HttpGet("")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionDetails == null)
            {
                return BadRequest();
            }
            else
            {
                _logger.Error($"\n\nPath = {exceptionDetails.Path}\tMessage = {exceptionDetails.Error.Message}\tStack Trace={exceptionDetails.Error.StackTrace}\n\n");

                return Problem(detail: _configuration["GamesGalleryConfigurations:StatusCodeErrorMessages:StatusCode500"], statusCode: 500, title: "Internal Server Error");
            }
        }
    }
}


