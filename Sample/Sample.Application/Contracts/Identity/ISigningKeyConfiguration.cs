using Microsoft.IdentityModel.Tokens;

namespace Sample.Application.Contracts.Identity
{
    public interface ISigningKeyConfiguration
    {
        SigningCredentials SigningCredentials { get; }
        SecurityKey SecurityKey { get; }
    }
}
