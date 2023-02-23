using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChargeSharedProto2.CustomExeptions;
using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChargeSharedProto2.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApplicationUser> RegisterApplicationUser(UserLoginAndRegisterDTO data)
        {
            var userExists = await _userManager.FindByEmailAsync(data.Email);
            if (userExists != null)
                throw new Exception("User already exists!");

            ApplicationUser newUser = new ApplicationUser
            {
                Email = data.Email,
                UserName = data.Email,
                IsValidUser = true,
                LockoutEnabled = false,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, data.Password);

            if (!result.Succeeded)
            {
                throw new Exception($"Could not register user! {result.Errors}");
            }

            return newUser;
        }

        public async Task<LoginResult> LoginApplicationUser(UserLoginAndRegisterDTO data)
        {
            var result = await _userManager.FindByEmailAsync(data.Email);
            if (result == null)
                throw new UserNotFoundExeption("Cannot find user by email");

            var loginResult = await _signInManager.PasswordSignInAsync(result, data.Password, true, false);

            if (!loginResult.Succeeded) throw new Exception($"Invalid password or email {loginResult}");

            await _userManager.AddClaimAsync(result, new Claim(ClaimTypes.Email, data.Email));

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, data.Email)
            };

            var jwtConfig = _configuration.GetSection("JWT");
            var secretKey = jwtConfig["secret"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(jwtConfig["expiresIn"]));

            var token = new JwtSecurityToken(
                jwtConfig["validIssuer"],
                jwtConfig["validAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new LoginResult
                { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token), Email = data.Email };
        }

        public async Task<ApplicationUser> ChangeUserData(UserDataDTO userData, string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                throw new Exception("Cannot find user");
            }

            user.FirstName = userData.FirstName;
            user.MiddleName = userData.MiddleName;
            user.LastName = userData.LastName;
            
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                throw new Exception("Cannot update user");
            }
            
        }
    }
}
