using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserManagementAPI.Services.Validation
{ 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAddressFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value?.ToString();

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmailFormat(email))
            {
                return new ValidationResult(ErrorMessage ?? "Invalid email format.");
            }

            return ValidationResult.Success;
        }

        private bool IsValidEmailFormat(string email)
        {
            try
            {
                // var addr = new System.Net.Mail.MailAddress(email);
                const string emailPattern = @"^(?!\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                return Regex.IsMatch(email, emailPattern);
               // return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}
