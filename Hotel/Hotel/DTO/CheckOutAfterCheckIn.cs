using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.DTO
{
    public class CheckOutAfterCheckInAttribute : ValidationAttribute
    {
        private readonly string _checkInPropertyName;

        public CheckOutAfterCheckInAttribute(string checkInPropertyName)
        {
            _checkInPropertyName = checkInPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checkOutDate = value as DateTime?;
            var checkInProperty = validationContext.ObjectType.GetProperty(_checkInPropertyName);

            if (checkInProperty == null)
            {
                return new ValidationResult($"Unknown property: {_checkInPropertyName}");
            }

            var checkInDate = checkInProperty.GetValue(validationContext.ObjectInstance, null) as DateTime?;

            if (checkInDate.HasValue && checkOutDate.HasValue && checkOutDate <= checkInDate)
            {
                return new ValidationResult(ErrorMessage ?? "Check-out date must be after check-in date.");
            }

            return ValidationResult.Success;
        }
    }

}