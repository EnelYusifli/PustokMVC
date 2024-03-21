using PustokTemp.Areas.Admin.ViewModels;

namespace PustokTemp.Business.Interfaces;

public interface IAuthService
{
    Task LoginAsync(AdminLoginViewModel adminLoginViewModel);
}
