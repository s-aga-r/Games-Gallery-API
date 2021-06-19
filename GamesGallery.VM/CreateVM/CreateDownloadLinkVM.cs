using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CreateVM
{
    public class CreateDownloadLinkVM
    {
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Link { get; set; }

        [Required]
        public Guid GameId { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}
