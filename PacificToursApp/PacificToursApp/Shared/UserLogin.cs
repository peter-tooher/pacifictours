using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // This class represents a user login form.
    public class UserLogin
    {
        // This property represents the user's username.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; } = string.Empty;

        // This property represents the user's password.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
