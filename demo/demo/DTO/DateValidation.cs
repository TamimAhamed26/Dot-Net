using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace demo.DTO
{
    public class DateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("EstablishedYear is required");
            }

            if (value is int year)
            {
                if (year < 1900 || year > DateTime.Now.Year)
                {
                    return new ValidationResult("EstablishedYear must be between 1900 and the current year");
                }
            }
            else
            {
                return new ValidationResult("Invalid year format");
            }

            return ValidationResult.Success;
        }
    }
}