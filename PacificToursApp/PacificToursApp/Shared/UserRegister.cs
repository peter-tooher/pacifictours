using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // This class represents a user registration form.
    public class UserRegister
    {
        // This property represents the user's username.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; } = string.Empty;

        // This property represents the user's first name.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "First Name is required."), Name]
        public string FirstName { get; set; } = string.Empty;

        // This property represents the user's last name.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "Last Name is required."), Name]
        public string LastName { get; set; } = string.Empty;

        // This property represents the user's email.
        // It is required and must be a valid email address.
        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = string.Empty;

        // This property represents the user's passport number.
        // It is required and cannot be empty.
        [Required(ErrorMessage = "Passport Number is required."), PassportNumber]
        public int PassportNumber { get; set; }

        // This property represents the user's contact number.
        // It is required and must be a valid phone number.
        [Required(ErrorMessage = "Contact Number is required."), Phone(ErrorMessage = "Contact Number is not valid.")]
        public string ContactNumber { get; set; } = string.Empty;

        // This property represents the user's password.
        // It is required and must be between 8 and 16 characters long.
        [Required(ErrorMessage = "Password is required."), StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        // This property represents the user's password confirmation.
        // It must match the password.
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}