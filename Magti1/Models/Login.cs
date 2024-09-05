using System.ComponentModel.DataAnnotations;

namespace Magti1.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Personal Id Number is required")]
        [Display(Name = "Personal Id Number")]
        public string PersonalIDNumber { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
