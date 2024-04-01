using System.ComponentModel.DataAnnotations;

namespace PustokTemp.Areas.Admin.ViewModels;

public class ResetPasswordViewModel
{
    public string Email { get; set; }
    public string Token { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
