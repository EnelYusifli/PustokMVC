using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokTemp.Business.Interfaces;
using PustokTemp.CustomExceptions.Common;
using PustokTemp.DAL;
using PustokTemp.Models;
using PustokTemp.ViewModels;

namespace PustokTemp.Business.Implementations;

public class AccService : IAccService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly PustokDbContext _context;

    public AccService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, PustokDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
    }
    public async Task LoginAsync(UserLoginViewModel userLoginViewModel)
    {
        AppUser? member = await _userManager.FindByNameAsync(userLoginViewModel.UserName);
        if (member is null)
            throw new EntityCannotBeFoundException("Invalid credentials");
        var result = await _signInManager.PasswordSignInAsync(member, userLoginViewModel.Password, false, false);
        if (!result.Succeeded)
            throw new NotSucceededException("Invalid credentials");
    }

    public async Task RegisterAsync(UserRegisterViewModel userRegisterViewModel)
    {
        AppUser member = new AppUser()
        {
            FullName = userRegisterViewModel.FullName,
            UserName = userRegisterViewModel.UserName,
            Email = userRegisterViewModel.Email
        };
        if (await _context.Users.AnyAsync(x => x.NormalizedUserName == member.UserName))
            throw new NameAlreadyExistException("UserName", "UserName is already exist");
        if (await _context.Users.AnyAsync(x => x.NormalizedEmail == member.Email))
        {
            throw new NameAlreadyExistException("Email", "Email is already exist");
        }
        var result = await _userManager.CreateAsync(member, userRegisterViewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                throw new NotSucceededException(err.Description);
            }
        }
        var roleResult = await _userManager.AddToRoleAsync(member, "Member");
        if (!roleResult.Succeeded)
        {
            foreach (var err in roleResult.Errors)
            {
                throw new NotSucceededException(err.Description);
            }
        }
    }
}
