using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesGallery.DL
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string CoverImage { get; set; }

        public float Size { get; set; }

        public int TotalDownloads { get; set; }

        public string MinimumRequirements { get; set; }

        public string RecommendedRequirements { get; set; }

        public string VideoTutorial { get; set; }

        public DateTime? DateOfUpload { get; set; }

        public int YearOfRelease { get; set; }

        public List<DownloadLink> DownloadLinks { get; set; }

        public List<Screenshot> Screenshots { get; set; }

        public virtual List<Category> Categories { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
