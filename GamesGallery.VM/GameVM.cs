using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM
{
    public class GameVM
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Cover Image")]
        public string CoverImage { get; set; }

        [Display(Name = "Size in GB")]
        public float Size { get; set; }

        [Display(Name = "Total Downloads")]
        public int TotalDownloads { get; set; }

        [Display(Name = "Minimum Requirements")]
        public string MinimumRequirements { get; set; }

        [Display(Name = "Recommended Requirements")]
        public string RecommendedRequirements { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Video Tutorial")]
        public string VideoTutorial { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Upload")]
        public DateTime? DateOfUpload { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Year Of Release")]
        public int YearOfRelease { get; set; }

        [Display(Name = "Download Links")]
        public List<DownloadLinkVM> DownloadLinks { get; set; }

        [Display(Name = "Screenshots")]
        public List<ScreenshotVM> Screenshots { get; set; }

        [Display(Name = "Categories")]
        public virtual List<CategoryVM> Categories { get; set; }

        [Display(Name = "Last Updated On")]
        public DateTime? LastUpdatedOn { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}
