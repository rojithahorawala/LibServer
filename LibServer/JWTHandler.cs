using MediaModel;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LibServer
{
    public class JWTHandler(UserManager<MediaUserModel> userManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateAsyncToken(MediaUserModel user)
        {
            return new JwtSecurityToken
            (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:ExpireInMinutes"])),
                claims: await GetClaimsAsync(user),   
                signingCredentials: GetSigningCredentials()
            );
        }
        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Convert.FromBase64String(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingKey = new(key);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaimsAsync(MediaUserModel user)
        {
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];

            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
