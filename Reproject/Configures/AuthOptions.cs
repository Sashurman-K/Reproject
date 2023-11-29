using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Reproject.Configures
{
    public class AuthOptions
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string SecretKey { get; init; }
        public int LifeTime { get; set; } 
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        }
    }
}

