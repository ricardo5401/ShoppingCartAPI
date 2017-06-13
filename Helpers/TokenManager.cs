using ShoppingCartApi.Extensions;
using ShoppingCartApi.Models;
using ShoppingCartApi.Options;
using System.IdentityModel.Tokens.Jwt;

namespace ShoppingCartApi.Helpers
{
    public static class TokenManager
    {
        public static TokenResponse BuildToken(TokenOptions options)
        {
            var token = new JwtSecurityToken(
                audience: options.Audience,
                issuer: options.Issuer,
                expires: options.GetExpiration(),
                signingCredentials: options.GetSigningCredentials());

            return new TokenResponse
            {
                token_type = options.Type,
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = (int) options.ValidFor.TotalSeconds
            };
        }
    }
}