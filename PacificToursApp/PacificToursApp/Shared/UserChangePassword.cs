using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class UserChangePassword
    {
        // The Password property represents the new password. It is required and its length must be between 8 and 16 characters.
        [Required, StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        // The ConfirmPassword property represents the confirmation of the new password. It must match the Password property.
        // If it doesn't match, an error message "The passwords do not match." will be returned.
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
