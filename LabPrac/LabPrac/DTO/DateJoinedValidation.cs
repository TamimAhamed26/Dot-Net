using System;
using System.ComponentModel.DataAnnotations;

namespace LabPrac.DTO
{
    public class DateJoinedValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now)
                {
                    return new ValidationResult("Date joined cannot be a future date.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
