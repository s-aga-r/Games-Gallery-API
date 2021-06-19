using GamesGallery.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GamesGallery.API.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : Controller
    {
        // Private Fields
        private readonly IGamesGalleryService service;


        // Public Constructor
        public ImagesController(IGamesGalleryService service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.service = service;
        }


        [HttpGet("{fileName}")]
        public IActionResult Get([FromRoute]string fileName)
        {
            FileStream image = service.GetImage(fileName);

            if(image == null)
            {
                return NotFound("The Image you are looking for is not available or have been removed.");
            }
            else
            {
                return File(image, "image/" + Path.GetExtension(fileName).Remove(0, 1));
            }
        }
    }
}
