using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{



    // The NameAttribute class is a custom validation attribute that validates names.
    // It inherits from the ValidationAttribute class.
    public class NameAttribute : ValidationAttribute
    {
        // The IsValid method is overridden to provide custom validation logic.
        // It takes two parameters: the value to validate and the context of the validation.
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // If the value is not null, it is converted to a string and checked against a regular expression.
            if (value != null)
            {
                string name = value.ToString();
                // The regular expression checks if the name consists only of letters.
                Regex regex = new Regex(@"^[a-zA-Z]+$");
                // If the name does not match the regular expression, a ValidationResult with an error message is returned.
                if (!regex.IsMatch(name))
                {
                    return new ValidationResult("Invalid name. Only letters are allowed.");
                }
            }
            // If the value is null or the name is valid, ValidationResult.Success is returned.
            return ValidationResult.Success;
        }
    }
}