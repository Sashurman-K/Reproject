using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Reproject.Configures;
using Reproject.Models.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Reproject.Processors
{
    public class ClaimsIdentityGenerator
    {
        private readonly AuthOptions _authOptions;
        private readonly NoteDbContext _dbContext;

        public ClaimsIdentityGenerator(AuthOptions authOptions, NoteDbContext dbContext)
        {
            _authOptions = authOptions;
            _dbContext = dbContext;
        }

        public User GetUserOrDefault(string userlogin, string password)
        {
            User user = _dbContext.UserDbSet.FirstOrDefault(item => item.UserLogin == userlogin);
            return PasswordHasher.Validate(user.UserPassword, password) ? user : null ;
        }
        public string GetJwt(User user, out ClaimsIdentity identity)
        {
            ///identity = GetIdentityOrDefault(userlogin, password);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserLogin)
            };
            identity = new ClaimsIdentity(
                claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(issuer: _authOptions.Issuer,
                                           audience: _authOptions.Audience,
                                           notBefore: now,
                                           claims: identity.Claims,
                                           expires: now.Add(TimeSpan.FromMinutes(_authOptions.LifeTime)),
                                           signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}