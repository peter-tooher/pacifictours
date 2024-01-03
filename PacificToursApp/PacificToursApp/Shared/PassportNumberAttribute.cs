using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    internal class PassportNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string passportNumber = value.ToString();
                Regex regex = new Regex(@"^\d+$");
                if (!regex.IsMatch(passportNumber))
                {
                    return new ValidationResult("Invalid passport number. Only numbers are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}