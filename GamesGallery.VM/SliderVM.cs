using System;

namespace GamesGallery.VM
{
    public class SliderVM
    {
        public Guid Id { get; set; }

        public string Image { get; set; }

        public Guid GameId { get; set; }

        public GameVM Game { get; set; }

        public DateTime? DateOfUpload { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
