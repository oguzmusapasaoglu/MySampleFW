using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MyCore.Common.Token;

namespace MyCore.TokenManager;

public class TokenHelper
{
    public static string GenerateToken(string userId, string userName, string userPages)
    {
        var jwtConfig = ConfigurationHelper.GetConfig<JwtConfigModel>(JwtConfigModel.SectionName);
        try
        {
            var expireDate = DateTime.Now.AddHours(5);
            var issuer = jwtConfig.Issuer;
            var audience = jwtConfig.Audience;
            var encryptionKey = Encoding.ASCII.GetBytes(jwtConfig.Key);

            var signinCredential = new SigningCredentials(
                new SymmetricSecurityKey(encryptionKey),
                SecurityAlgorithms.HmacSha256);

            var userClaims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, userPages)
            };
            var jwToken = new JwtSecurityToken(issuer: issuer,
                                           audience: audience,
                                           claims: userClaims,
                                           notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                                           expires: expireDate,
                                           signingCredentials: signinCredential
        );

            return new JwtSecurityTokenHandler().WriteToken(jwToken);
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex);
        }
    }
    public static List<Claim> GetClaimsFromToken(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.Claims.ToList();
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex);
        }
    }
    public static int GetUserIdFromToken(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string userId = jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value;
            return userId.ToInt();
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex);
        }
    }
    public static string GetUserNameFromToken(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string userId = jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            return userId;
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex);
        }
    }
    public static int ControlToken(string authValue, int requestUserID)
    {
        if (authValue.IsNotNullOrEmpty())
        {
            var token = authValue.Split(' ')[1];
            var claims = GetClaimsFromToken(token);
            int tokenUserID = claims.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.UniqueName).Value.ToInt();
            if (requestUserID != tokenUserID)
                throw new CustomException(ExceptionMessageHelper.TokenException);           
            return requestUserID;
        }
        throw new CustomException(ExceptionMessageHelper.TokenException);
    }
}
