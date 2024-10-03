using Magti1.Models;
using Microsoft.AspNetCore.Identity;

namespace Magti1.ViewModels;

public class AssignRolesViewModel
{
    public List<ApplicationUser> Users { get; set; }
    public List<IdentityRole<int>> Roles { get; set; }
}
