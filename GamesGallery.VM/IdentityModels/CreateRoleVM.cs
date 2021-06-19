using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesGallery.VM.IdentityModels
{
    public class CreateRoleVM
    {
        [Required]
        public string RollName { get; set; }
    }
}
