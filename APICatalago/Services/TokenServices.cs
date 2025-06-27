using APICatalago.Services.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace APICatalago.Services;

public class TokenServices : ITokenService
{
    public JwtSecurityToken GenerateAccessToken ( IEnumerable<Claim> claims , IConfiguration _config )
    {
        var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Invalid secret Key");

        var privateKey = Encoding.UTF8.GetBytes(key);

        //uso a chave para poder criar a credencial no qual vai assinar
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey) ,
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims) ,
            Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")) ,
            Audience = _config.GetSection("JWT").GetValue<string>("ValidateAudience") ,
            Issuer = _config.GetSection("JWT").GetValue<string>("ValidateIssuer") ,
            SigningCredentials = signingCredentials
        };

        var tokenHnadler = new JwtSecurityTokenHandler();
        var token = tokenHnadler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken ()
    {
        var secureRandomBytes = new byte [128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        //fAÇO ISSO PRA TORNAR MAIS SUAVE A TRANSIÇAO DESSA INFO
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken ( string token , IConfiguration _config )
    {
        var secretKey = _config ["JWT:ScretKey"] ?? throw new InvalidOperationException("Invalid Key");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false ,
            ValidateIssuer = false ,
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) ,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token , tokenValidationParameters , out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
            SecurityAlgorithms.HmacSha256 , StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ArgumentException("Invalid token");
        }
        return principal;
    }
}