using System.ComponentModel.DataAnnotations;

namespace Magti1.Models
{
    public class Register
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Personal Id Number")]
        public string PersonalIDNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords does not match")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? ImageFileName { get; set; }
        [Display(Name = "Image of your personnal id")]
        public IFormFile ImageFile { get; set; }
    }
}
