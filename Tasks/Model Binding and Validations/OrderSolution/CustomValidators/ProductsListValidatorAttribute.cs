using OrderSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderSolution.CustomValidators
{
    public class ProductsListValidatorAttribute : ValidationAttribute
    {
        public string DefaultErrorMessage { get; set; } = "Order should have at least one product";

        public ProductsListValidatorAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                List<Product> products = (List<Product>)value;

                if (products.Count == 0)
                {
                    return new ValidationResult(DefaultErrorMessage, new string[] { nameof(validationContext.MemberName) });
                }

                return ValidationResult.Success;
            }
            return null;
        }
    }
}
