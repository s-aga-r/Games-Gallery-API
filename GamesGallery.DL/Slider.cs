using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesGallery.DL
{
    public class Slider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Image { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }

        public DateTime? DateOfUpload { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
