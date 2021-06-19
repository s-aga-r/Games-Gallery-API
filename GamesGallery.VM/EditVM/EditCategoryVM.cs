using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.EditVM
{
    public class EditCategoryVM
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        ////Helper Property
        //[Display(Name = "Games")]
        //public List<Guid> GamesId { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }
    }
}
