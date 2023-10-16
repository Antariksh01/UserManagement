using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagementAPI.Services.Validation;

namespace UserManagementAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name must not contain numbers.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name must not contain numbers.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        //Below Annotation id default check
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        //Below Annotations are customized format check
        [UniqueEmail(ErrorMessage = "Email address must be unique.")]
        [EmailAddressFormat(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string Notes { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
