using System;

namespace GamesGallery.VM
{
    public class ScreenshotVM
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public Guid GameId { get; set; }

        public GameVM Game { get; set; }

        public bool IsActive { get; set; }
    }
}
