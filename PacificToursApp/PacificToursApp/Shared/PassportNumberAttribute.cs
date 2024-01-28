using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The PassportNumberAttribute class is a custom validation attribute that validates passport numbers.
    // It inherits from the ValidationAttribute class.
    internal class PassportNumberAttribute : ValidationAttribute
    {
        // The IsValid method is overridden to provide custom validation logic.
        // It takes two parameters: the value to validate and the context of the validation.
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // If the value is not null, it is converted to a string and checked against a regular expression.
            if (value != null)
            {
                string passportNumber = value.ToString();
                // The regular expression checks if the passport number consists only of digits.
                Regex regex = new Regex(@"^\d+$");
                // If the passport number does not match the regular expression, a ValidationResult with an error message is returned.
                if (!regex.IsMatch(passportNumber))
                {
                    return new ValidationResult("Invalid passport number. Only numbers are allowed.");
                }
            }
            // If the value is null or the passport number is valid, ValidationResult.Success is returned.
            return ValidationResult.Success;
        }
    }
}