using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalago.Services.Tokens;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken ( IEnumerable<Claim> claims , IConfiguration _config );

    string GenerateRefreshToken ();

    ClaimsPrincipal GetClaimsPrincipalFromExpiredToken ( string token , IConfiguration _config );
}