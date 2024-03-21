using PustokTemp.ViewModels;

namespace PustokTemp.Business.Interfaces;

public interface IAccService
{
    Task RegisterAsync(UserRegisterViewModel userRegisterViewModel);
    Task LoginAsync(UserLoginViewModel userLoginViewModel);
}
