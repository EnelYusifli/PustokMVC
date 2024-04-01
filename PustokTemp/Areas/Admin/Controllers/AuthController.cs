using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokTemp.Areas.Admin.ViewModels;
using PustokTemp.Business.Interfaces;
using PustokTemp.CustomExceptions.Common;
using PustokTemp.DAL;
using PustokTemp.Models;
using System.Net.Mail;
using System.Net;

namespace PustokTemp.Areas.Admin.Controllers;
[Area("Admin")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly PustokDbContext _context;

    public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, PustokDbContext context, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
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

    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
    {
        if (!ModelState.IsValid) return View();
        var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

        if (user is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("ResetPassword", "Auth", new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);


            //MailAddress to = new MailAddress(forgotPasswordVM.Email);
            //MailAddress from = new MailAddress("yusiflienel@gmail.com");

            //MailMessage email = new MailMessage(from, to);
            //email.Subject = "Testing out email sending";
            //email.Body = link;

            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 25;
            //smtp.Credentials = new NetworkCredential("smtp_username", "smtp_password");
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.EnableSsl = true;
            //smtp.Send(email);

            return View("InfoPage");
        }
        else
        {
            ModelState.AddModelError("Email", "User not found");
            return View();
        }
    }

    public IActionResult ResetPassword(string email,string token)
    {
        if(email == null || token == null) return NotFound();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
    {
        if (!ModelState.IsValid)return View(); 
        var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
        if (user is not null)
        {
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }
        }
        else return NotFound();

        return RedirectToAction(nameof(Login));
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
