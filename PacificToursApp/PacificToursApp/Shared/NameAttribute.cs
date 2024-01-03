using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{



    public class NameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string name = value.ToString();
                Regex regex = new Regex(@"^[a-zA-Z]+$"); 
                if (!regex.IsMatch(name))
                {
                    return new ValidationResult("Invalid name. Only letters are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}