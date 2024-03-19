using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokTemp.Areas.Admin.ViewModels;
using PustokTemp.Models;

namespace PustokTemp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize (Roles = "SuperAdmin,Admin")]
public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel adminLoginViewModel)
    {
        if (!ModelState.IsValid) return View();
       AppUser? superAdmin= await _userManager.FindByNameAsync(adminLoginViewModel.UserName);
        if(superAdmin is null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
       var result= await _signInManager.PasswordSignInAsync(superAdmin,adminLoginViewModel.Password,false,false);
        if(!result.Succeeded) {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
        return RedirectToAction("Index","Dashboard");
    }

    //public async Task<IActionResult> CreateAdmin()
    //{
    //    AppUser superAdmin = new AppUser()
    //    {
    //        FullName="SuperAdmin",
    //        UserName="SuperAdmin",
    //        Email="superadmin@gmail.com"
    //    };
    //   var result= await _userManager.CreateAsync(superAdmin,"SuperAdmin123!");

    //    return Ok(result);
    //}
    //public async Task<IActionResult> CreateRole()
    //{
    //    IdentityRole superAdminRole = new IdentityRole("SuperAdmin");
    //    IdentityRole adminRole = new IdentityRole("Admin");
    //    IdentityRole memberRole = new IdentityRole("Member");
    //    await _roleManager.CreateAsync(superAdminRole);
    //    await _roleManager.CreateAsync(adminRole);
    //    await _roleManager.CreateAsync(memberRole);
    //    return Ok();
    //}
    //public async Task<IActionResult> AddRole()
    //{
    //    AppUser superAdmin = await _userManager.FindByNameAsync("SuperAdmin");
    //   var result= await _userManager.AddToRoleAsync(superAdmin,"SuperAdmin");
    //    return Ok(result);  
    //}
}
