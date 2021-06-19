using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.IdentityModels
{
    public class SignUpVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "EmailAvailable", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
