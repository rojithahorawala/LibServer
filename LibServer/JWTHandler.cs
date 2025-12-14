using MediaModel;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace LibServer
{
    public class JWTHandler(UserManager<MediaUserModel> userManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateTokenAsync(MediaUserModel user)
        {
            return new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:ExpireInMinutes"])),
                claims: await GetClaimAsync(user),
                signingCredentials: GetSigningCredentials()
            );

        }
        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingKey = new(key);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaimAsync(MediaUserModel user)
        {
            // implement for getting claims
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];
            //claims.AddRange((await UserManager.GettRolesAsync(user)).Select(RoleManager => claims(ClaimTypes.Role, RoleManager)));
            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
