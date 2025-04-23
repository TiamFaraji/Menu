using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]

        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "the password and the confirmation password doesnt match")]
        public required string PasswordConfirmation { get; set; }
    }
}
