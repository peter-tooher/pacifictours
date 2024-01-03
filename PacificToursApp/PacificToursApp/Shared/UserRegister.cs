using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class UserRegister
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "First Name is required."), Name]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last Name is required."), Name]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Passport Number is required."), PassportNumber]
        public int PassportNumber { get; set; }
        [Required(ErrorMessage = "Contact Number is required."), Phone(ErrorMessage = "Contact Number is not valid.")]
        public string ContactNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required."), StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}