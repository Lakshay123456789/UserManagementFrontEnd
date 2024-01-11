using System.ComponentModel.DataAnnotations;

namespace UserManagementFrontEnd.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name must not exceed 50 characters")]
        public string? FirstName { get; set; }


        [StringLength(50, ErrorMessage = "Last Name must not exceed 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact must be 10 digits")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Contact must be numeric")]
        public string? Email { get; set; }
    }
}
