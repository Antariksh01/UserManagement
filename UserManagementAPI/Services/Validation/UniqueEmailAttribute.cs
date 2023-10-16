using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Data;
using UserManagementAPI.Services.UserService;

namespace UserManagementAPI.Services.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueEmailAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (DataContext)validationContext.GetService(typeof(DataContext));
            var currentValue = value?.ToString();

            // Check if the email address is unique
            if (dbContext.Users.Any(u => u.Email == currentValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}
