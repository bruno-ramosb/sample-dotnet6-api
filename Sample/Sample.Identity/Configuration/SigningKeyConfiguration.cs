using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sample.Application.Contracts.Identity;

namespace Sample.Identity.Configuration
{
    public class SigningKeyConfiguration : ISigningKeyConfiguration
    {
        public SigningCredentials SigningCredentials { get; }
        public SecurityKey SecurityKey { get; }

        public SigningKeyConfiguration(string secretKey)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            SecurityKey = securityKey;

            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
