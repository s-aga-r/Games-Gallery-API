using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CreateVM
{
    public class CreateCategoryVM
    {
        [Required]
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
