using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LabPrac.DTO
{
    public class CategoryValidationAttribute : ValidationAttribute
    {
        private readonly string[] validCategories = { "Electronics", "Clothing", "Books", "Food", "home appliances" };

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return Array.Exists(validCategories, category =>
                category.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
