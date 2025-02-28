
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Tourism_Api.Entity.user;

namespace Tourism_Api.Outherize;

public class token(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expiresIn) GenerateToken(UserRespones user)
    {
        // Define claims for the token
        Claim[] claims = new[]
        {
           
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.Name !),
            new Claim("Password", user.Password !),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //// Create the key for signing the token
        //var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjacB"));

        //// Define signing credentials with security algorithm
        //var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


        ////
        //var expiresIn = 30;

        //var token = new JwtSecurityToken(
        //    issuer: "SurveyBasketApp",
        //    audience: "SurveyBasketApp users",
        //    claims: claims,
        //    expires: DateTime.UtcNow.AddMinutes(expiresIn),
        //    signingCredentials: signingCredentials
        //);

        // use option pattern

        // Create the key for signing the token
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        // Define signing credentials with security algorithm
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,    
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );



        //return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _options.ExpiryMinutes * 60);

    }

    public string? ValisationToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            tokenHandler.ValidateToken(token, validationParameters: new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
        }
        catch
        {
            return null;
        }
    }

}
