using Microsoft.AspNetCore.Identity;

namespace Magti1.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    //To Make AspNetUsers Id int, not guid
    public class ApplicationUser : IdentityUser<int>
    {
        public string? PersonalIDNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ImageFileName { get; set; }
    }
}
