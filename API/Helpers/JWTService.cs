using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Helpers
{
    public class JWTService
    {
        /// <summary>
        /// Generates a JWT 
        /// </summary>
        /// <param name="id">Id of the user to Store in the payload of the JWT</param>
        /// <param name="securityKey">Key to generate JT</param>
        /// <returns><Returns a new JWT</returns>
        public string GenerateJWT(int? id, string securityKey)
        {
            //We need to get Security key
            var bytesSymmetricSecurityKey = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(securityKey));

            //Get Credentials, and use the algorithm
            var credentials = new SigningCredentials(bytesSymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(credentials);

            //Payload to store on the JWT, first param is the issuer or data we want to store
            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));

            //Combine header with the payload to generate JWT
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }

        public JwtSecurityToken VerifyJWT(string jwt, string securityKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
