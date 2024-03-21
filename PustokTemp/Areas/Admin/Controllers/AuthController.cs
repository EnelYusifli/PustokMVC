using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokTemp.Areas.Admin.ViewModels;
using PustokTemp.Business.Interfaces;
using PustokTemp.CustomExceptions.Common;
using PustokTemp.Models;

namespace PustokTemp.Areas.Admin.Controllers;
[Area("Admin")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
        try
        {
            await _authService.LoginAsync(adminLoginViewModel);
        }
        catch (EntityCannotBeFoundException ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        } 
        catch (NotSucceededException ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        }
        return RedirectToAction("Index","Dashboard");
    }

    //public async Task<IActionResult> CreateAdmin()
    //{
    //    AppUser superAdmin = new AppUser()
    //    {
    //        FullName = "SuperAdmin",
    //        UserName = "SuperAdmin",
    //        Email = "superadmin@gmail.com"
    //    };
    //    var result = await _userManager.CreateAsync(superAdmin, "SuperAdmin123!");

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
    //    var result = await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
    //    return Ok(result);
    //}
}
