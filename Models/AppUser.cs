using Microsoft.AspNetCore.Identity;

namespace securityapp.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = "";
}