using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokTemp.Areas.Admin.ViewModels;
using PustokTemp.DAL;
using PustokTemp.Models;
using PustokTemp.ViewModels;

namespace PustokTemp.Controllers;

public class AccController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly PustokDbContext _context;

    public AccController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager,PustokDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
    {
        if (!ModelState.IsValid)return View();
        AppUser member = new AppUser()
        {
            FullName = userRegisterViewModel.FullName,
            UserName = userRegisterViewModel.UserName,
            Email = userRegisterViewModel.Email
        };
        if(await _context.Users.AnyAsync(x => x.NormalizedUserName == member.UserName))
        {
            ModelState.AddModelError("UserName", "UserName is already exist");
            return View();
        }
        if (await _context.Users.AnyAsync(x => x.NormalizedEmail == member.Email))
        {
            ModelState.AddModelError("Email", "Email is already exist");
            return View();
        }
        var result= await _userManager.CreateAsync(member, userRegisterViewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors) { 
            ModelState.AddModelError("",err.Description);
            }
            return View();
        }
        var roleResult = await _userManager.AddToRoleAsync(member, "Member");
        if (!roleResult.Succeeded)
        {
            foreach (var err in roleResult.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return View();
        }
        return RedirectToAction("index","home");
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
    {
        if (!ModelState.IsValid) return View();
        AppUser? member = await _userManager.FindByNameAsync(userLoginViewModel.UserName);
        if (member is null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
        var result = await _signInManager.PasswordSignInAsync(member, userLoginViewModel.Password, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

}
