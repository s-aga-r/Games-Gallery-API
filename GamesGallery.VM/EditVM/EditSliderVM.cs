using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.EditVM
{
    public class EditSliderVM
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile ImageIFormFile { get; set; }

        public Guid? GameId { get; set; }

        public int? Order { get; set; }

        [Display(Name = "Active Status")]
        public bool? IsActive { get; set; }
    }
}
