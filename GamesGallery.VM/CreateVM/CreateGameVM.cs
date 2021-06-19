using GamesGallery.VM.CustomValidationAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CreateVM
{
    public class CreateGameVM
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // Helper Property
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageIFormFile { get; set; }

        [Required]
        [Display(Name = "Size in GB")]
        [Range(0, float.MaxValue, ErrorMessage = "The field Size must be greater than or equal to 0.")]
        public float Size { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Minimum Requirements")]
        public string MinimumRequirements { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Recommended Requirements")]
        public string RecommendedRequirements { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Video Tutorial")]
        public string VideoTutorial { get; set; }

        [Required]
        [YearOfReleaseValidation]
        [Display(Name = "Year of Release")]
        public int YearOfRelease { get; set; }

        //Helper Property
        [Required]
        [Display(Name = "Download Links")]
        public string DownloadLinksString { get; set; }

        // Helper Property
        [DataType(DataType.Upload)]
        [Display(Name = "Screenshots")]
        public List<IFormFile> ScreenshotsIFormFile { get; set; }

        //Helper Property
        [Required]
        [Display(Name = "Categories")]
        public List<Guid> CategoriesId { get; set; }

        //Helper Property
        [Display(Name = "Categories")]
        public List<SelectListItem> CategoriesSelectList { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        public CreateGameVM()
        {
            this.MinimumRequirements = "Operating System : Windows 7\nProcessor : Intel i3 3rd Gen\nRAM : 2GB\nFree HardDisk Space : 10GB";
            this.RecommendedRequirements = "Operating System : Windows 8.1 or Above\nProcessor : Intel i5 3rd Gen or Above\nRAM : 4GB or Above\nFree HardDisk Space : 10GB or Above";
            this.YearOfRelease = DateTime.Now.Year;
            this.IsActive = true;
        }
    }
}
