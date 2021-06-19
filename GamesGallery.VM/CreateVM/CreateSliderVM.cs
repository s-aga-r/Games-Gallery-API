using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CreateVM
{
    public class CreateSliderVM
    {
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile ImageIFormFile { get; set; }

        [Required]
        public Guid GameId { get; set; }

        [Required]
        public int Order { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}
