using System;
using System.ComponentModel.DataAnnotations;

namespace GamesGallery.VM.CustomValidationAttribute
{
    public class YearOfReleaseValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value != null)
            {
                int YearOfRelease = (int)value;

                if (YearOfRelease > DateTime.Now.Year || YearOfRelease < DateTime.Now.AddYears(-25).Year)
                {
                    ErrorMessage = $"Please enter a value between {DateTime.Now.AddYears(-25).Year} to {DateTime.Now.Year}.";
                    return false;
                }
            }

            return true;
        }
    }
}
