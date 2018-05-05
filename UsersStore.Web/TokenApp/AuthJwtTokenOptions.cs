using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UsersStore.Web.TokenApp
{
    public static class AuthJwtTokenOptions
    {
        public const string Issuer = "MyProvider";

        public const string Audience = "http://localhost:54677";

        private const string Key = "supersecret_secretkey!12345";

        public static SecurityKey GetSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
