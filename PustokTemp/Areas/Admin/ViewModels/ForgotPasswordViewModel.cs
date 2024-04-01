using System.ComponentModel.DataAnnotations;

namespace PustokTemp.Areas.Admin.ViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
