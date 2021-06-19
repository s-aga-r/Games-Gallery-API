using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.EditVM
{
    public class EditScreenshotVM
    {
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile ImageIFormFile { get; set; }

        public Guid? GameId { get; set; }

        [Display(Name = "Active Status")]
        public bool? IsActive { get; set; }
    }
}
