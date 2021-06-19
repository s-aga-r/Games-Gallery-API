using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CreateVM
{
    public class CreateScreenshotVM
    {
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile ImageIFormFile { get; set; }

        [Required]
        public Guid GameId { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}
