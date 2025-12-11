using LibServer.DTO;
using MediaModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;

namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController 
    (
    UserManager<MediaUserModel> userManager,
    JWTHandler JWTHandler
    )
        
    : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest) 
        {
            MediaUserModel? mediaUser = await userManager.FindByNameAsync(loginRequest.Username);
            if (mediaUser == null)
            {
                return Unauthorized("Username is Invalid");
            }
            bool loginStatus = await userManager.CheckPasswordAsync(mediaUser, loginRequest.Password);
            if (!loginStatus)
            {
                return Unauthorized("Password is Invalid");
            }


            JwtSecurityToken jwtToken = await JWTHandler.GenerateAsyncToken(mediaUser);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok
            (
            new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = stringToken
            }
            );
        }
    }
}
