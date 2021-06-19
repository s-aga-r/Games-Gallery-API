using GamesGallery.VM.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.EditVM
{
    public class EditGameVM
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MinLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // Helper Property
        [DataType(DataType.Upload)]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageIFormFile { get; set; }

        [Display(Name = "Size in GB")]
        [Range(0, float.MaxValue, ErrorMessage = "The field Size must be greater than or equal to 0.")]
        public float? Size { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Minimum Requirements")]
        public string MinimumRequirements { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Recommended Requirements")]
        public string RecommendedRequirements { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Video Tutorial")]
        public string VideoTutorial { get; set; }

        [YearOfReleaseValidation]
        [Display(Name = "Year of Release")]
        public int? YearOfRelease { get; set; }

        //Helper Property
        [Display(Name = "Download Links")]
        public string DownloadLinksString { get; set; }

        // Helper Property
        [DataType(DataType.Upload)]
        [Display(Name = "Screenshots")]
        public List<IFormFile> ScreenshotsIFormFile { get; set; }

        //Helper Property
        [Display(Name = "Categories")]
        public List<Guid> CategoriesId { get; set; }

        // Helper Property
        //[Display(Name = "Categories")]
        //public List<SelectListItem> CategoriesSelectList { get; set; }

        [Display(Name = "Active Status")]
        public bool? IsActive { get; set; }
    }
}
