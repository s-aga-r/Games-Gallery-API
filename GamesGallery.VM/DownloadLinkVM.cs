using System;

namespace GamesGallery.VM
{
    public class DownloadLinkVM
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public Guid GameId { get; set; }

        public GameVM Game { get; set; }

        public bool IsActive { get; set; }
    }
}
