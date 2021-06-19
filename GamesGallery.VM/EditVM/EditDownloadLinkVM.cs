using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.EditVM
{
    public class EditDownloadLinkVM
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [DataType(DataType.Url)]
        public string Link { get; set; }

        public Guid? GameId { get; set; }

        [Display(Name = "Active Status")]
        public bool? IsActive { get; set; }
    }
}
