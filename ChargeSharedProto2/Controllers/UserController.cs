using System.IdentityModel.Tokens.Jwt;
using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ChargeSharedProto2.Models;
using Microsoft.AspNetCore.Authorization;
using Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChargeSharedProto2.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IEnumerable<IdentityError> _errors { get; set; }


        public UserController(IUserService userService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: api/<UserController>
        [Authorize]
        [HttpGet("{email}")]
        public async Task<ApplicationUser> Get(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return user;
        }


        // POST api/<UserController>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterPost([FromBody] UserLoginAndRegisterDTO data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.RegisterApplicationUser(data);
                    var loginResult = await _userService.LoginApplicationUser(data);
                    return Ok(loginResult);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                foreach (var error in _errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(_errors);
            }
        }

        /*---------------------------------------------------------------------------------------------------------*/

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginPost([FromBody] UserLoginAndRegisterDTO data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.LoginApplicationUser(data);

                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                foreach (var error in _errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest("Could not save charging station!");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{email}")]
        public async Task<IActionResult> Put(string email, [FromBody] UserDataDTO value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.ChangeUserData(value, email);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                
            }

            return BadRequest("Model not valid");
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
