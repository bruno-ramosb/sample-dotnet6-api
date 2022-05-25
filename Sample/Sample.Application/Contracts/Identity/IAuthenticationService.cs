using Sample.Application.Models.Authentication;
using Sample.Domain.Entities.Identities;
using System.Security.Claims;

namespace Sample.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateWithAzureAdAsync(string accessToken);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        Task<AuthenticationResponse> AuthenticateWithPasswordAsync(AuthenticationRequest authenticationRequest);
        Task<AuthenticationResponse> AuthenticateWithRefreshTokenAsync(string refreshToken);
        JwtToken GenerateToken(IEnumerable<Claim> claims, int? expiresIn = null);
        Task<bool> VerifiyToken(string token);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);

    }
}
