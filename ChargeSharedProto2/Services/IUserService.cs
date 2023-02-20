using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Services;

public interface IUserService
{
    Task<ApplicationUser> RegisterApplicationUser(UserLoginAndRegisterDTO data);
    Task<LoginResult> LoginApplicationUser(UserLoginAndRegisterDTO data);
    Task<ApplicationUser> ChangeUserData(UserDataDTO userData, string email);
}