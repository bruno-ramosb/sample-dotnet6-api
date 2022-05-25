using Sample.Domain.Entities.Identities;

namespace Sample.Application.Contracts.Identity
{
    public interface IRefreshTokenService
    {
        Task<bool> AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByToken(string refreshToken);
        Task DeleteExpiredTokens();
    }
}
