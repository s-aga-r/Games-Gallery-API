using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesGallery.DL
{
    public class DownloadLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public string Link { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }

        public bool IsActive { get; set; }
    }
}
