using System;
using System.Collections.Generic;

namespace GamesGallery.VM
{
    public class CategoryVM
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual List<GameVM> Games { get; set; }

        public bool IsActive { get; set; }
    }
}
