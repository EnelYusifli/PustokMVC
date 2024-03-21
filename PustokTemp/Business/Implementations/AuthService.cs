using Microsoft.AspNetCore.Identity;
using PustokTemp.Areas.Admin.ViewModels;
using PustokTemp.Business.Interfaces;
using PustokTemp.CustomExceptions.Common;
using PustokTemp.Models;

namespace PustokTemp.Business.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    public async Task LoginAsync(AdminLoginViewModel adminLoginViewModel)
    {
        AppUser? admin = await _userManager.FindByNameAsync(adminLoginViewModel.UserName);
        if (admin is null)
            throw new EntityCannotBeFoundException("Invalid credentials");
        var result = await _signInManager.PasswordSignInAsync(admin, adminLoginViewModel.Password, false, false);
        if (!result.Succeeded)
            throw new NotSucceededException("Invalid credentials");
    }
}
