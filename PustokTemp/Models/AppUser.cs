using Microsoft.AspNetCore.Identity;

namespace PustokTemp.Models;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }
}
