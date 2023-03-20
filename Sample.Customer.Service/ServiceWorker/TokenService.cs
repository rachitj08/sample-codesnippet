using Common.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Sample.Customer.Model;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface ITokenService
    {
        AuthResultModel GenerateAuthToken(User user);
        string GenerateJwtToken(User user, out Guid tokenId);
        string VerifyToken(RefreshTokenModel storedToken);
        ResponseResult<long> VerifyJwtToken(string token);
    }

    public class TokenService: ITokenService
    {
        /// <summary>
        ///  Load Authentication configuration
        /// </summary>
        private readonly AuthenticationConfig authenticationConfig;

        public TokenService(IOptions<AuthenticationConfig> authenticationConfig)
        {
            this.authenticationConfig = authenticationConfig.Value;
        } 

        public AuthResultModel GenerateAuthToken(User user)
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
                AccountId = user.AccountId,
                JwtToken = jwtToken
            };

            return new AuthResultModel()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken
            };
        }

        public string GenerateJwtToken(User user, out Guid tokenId)
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
                // Validation 1 - Validation JWT token format
                //var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = new SymmetricSecurityKey(key),
                //    ValidateIssuer = false,
                //    ValidateAudience = false,
                //    ValidateLifetime = false,
                //    RequireExpirationTime = false
                //}, out var validatedToken);

                //// Validation 2 - Validate encryption alg
                //if (validatedToken is JwtSecurityToken jwtSecurityToken)
                //{
                //    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                //    if (result == false)
                //    {
                //        return "Token has not yet expired";
                //    }
                //}

                //// Validation 3 - Validate expiry date
                //var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                //var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                //if (expiryDate > DateTime.UtcNow)
                //{
                //    return "Token has not yet expired";
                //}

                // validation 4 - Validate existence of the token
                if (storedToken == null)
                {
                    return "Token does not exist";
                }

                // Validation 5 - Validate if used
                if (storedToken.IsUsed)
                {
                    return "Token has been used";
                }

                // Validation 6 - Validate if revoked
                if (storedToken.IsRevorked)
                {
                    return "Token has been revoked";
                }

                // Validation 7 - Validate the id
                //var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                //if (Guid.TryParse(jti, out var jtiValue) || storedToken.JwtId != jtiValue)
                //{
                //    return "Token doesn't match";
                //}
                 
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

        public ResponseResult<long> VerifyJwtToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(authenticationConfig.ClientSecret);

                SecurityToken validatedToken;
                var validator = new JwtSecurityTokenHandler();

                // These need to match the values used to generate the token
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ValidIssuer = authenticationConfig.Issuer,
                };

                if (validator.CanReadToken(token))
                {
                    ClaimsPrincipal principal;

                    // This line throws if invalid
                    principal = validator.ValidateToken(token, validationParameters, out validatedToken);


                    // Validation 3 - Validate expiry date
                    if (principal.HasClaim(c => c.Type == JwtRegisteredClaimNames.Exp))
                    {
                        var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                        var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                        if (expiryDate < DateTime.UtcNow)
                        {
                            //Token has been expired.;
                            return new ResponseResult<long>()
                            {
                                Message = ResponseMessage.Unauthorized,
                                ResponseCode = ResponseCode.Unauthorized,
                                Error = new ErrorResponseResult()
                                {
                                    Message = "Token has been expired."
                                }
                            };
                        }
                    }
                    else
                    {
                        return new ResponseResult<long>()
                        {
                            Message = ResponseMessage.Unauthorized,
                            ResponseCode = ResponseCode.Unauthorized,
                            Error = new ErrorResponseResult()
                            {
                                Message = "Invalid token"
                            }
                        };
                    }

                    // If we got here then the token is valid
                    if (principal.HasClaim(c => c.Type == "UserId")
                        && Int64.TryParse(principal.Claims.Where(c => c.Type == "UserId").First().Value, out var userId)
                        && userId > 0)
                    {
                        return new ResponseResult<long>()
                        {
                            Message = ResponseMessage.RecordFetched,
                            ResponseCode = ResponseCode.RecordFetched,
                            Data = userId,
                        };
                    }
                    else
                    {
                        return new ResponseResult<long>()
                        {
                            Message = ResponseMessage.Unauthorized,
                            ResponseCode = ResponseCode.Unauthorized,
                            Error = new ErrorResponseResult()
                            {
                                Message = "Invalid token"
                            }
                        };
                    }

                }
                else
                {
                    return new ResponseResult<long>()
                    {
                        Message = ResponseMessage.Unauthorized,
                        ResponseCode = ResponseCode.Unauthorized,
                        Error = new ErrorResponseResult()
                        {
                            Message = "Invalid token"
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult<long>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ex.Message
                    }
                };
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

