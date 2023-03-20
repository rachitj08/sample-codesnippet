using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sample.Admin.Service.ServiceWorker
{
    public class TokenService : ITokenService
    {
        /// <summary>
        ///  Load Authentication configuration
        /// </summary>
        private readonly AuthenticationConfig authenticationConfig;

        public TokenService(IOptions<AuthenticationConfig> authenticationConfig)
        {
            this.authenticationConfig = authenticationConfig.Value;
        }

        public AuthResultModel GenerateAuthToken(AdminUsers user)
        {
            var jwtToken = GenerateJwtToken(user, out var tokenId);

            var refreshToken = new RefreshTokenModel()
            {
                JwtId = tokenId,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.UserId,
                ExpiryDate = DateTime.UtcNow.AddHours(authenticationConfig.RefreshExpiryTime),
                Token = RandomString(35) + Guid.NewGuid(),
                JwtToken = jwtToken
            };

            return new AuthResultModel()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken
            };
        }

        public string GenerateJwtToken(AdminUsers user, out Guid tokenId)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(authenticationConfig.ClientSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = authenticationConfig.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddMinutes(authenticationConfig.ExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            tokenId = Guid.Parse(token.Id);
            return jwtTokenHandler.WriteToken(token);
        }

        public string VerifyToken(RefreshTokenModel storedToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.ASCII.GetBytes(authenticationConfig.ClientSecret);
                
                // validation 1 - Validate existence of the token
                if (storedToken == null)
                {
                    return "Token does not exist";
                }

                // Validation 2 - Validate if used
                if (storedToken.IsUsed)
                {
                    return "Token has been used";
                }

                // Validation 3 - Validate if revoked
                if (storedToken.IsRevorked)
                {
                    return "Token has been revoked";
                }

                return "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return "Token has expired please re-login";
                }
                else
                {
                    return ResponseMessage.InternalServerError;
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
