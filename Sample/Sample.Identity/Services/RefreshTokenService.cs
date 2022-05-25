using Microsoft.EntityFrameworkCore;
using Sample.Application.Contracts.Identity;
using Sample.Domain.Entities.Identities;

namespace Sample.Identity.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly SampleIdentityDbContext _dbContext;

        public RefreshTokenService(SampleIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddAsync(RefreshToken refreshToken)
        {
            await _dbContext.Set<RefreshToken>().AddAsync(refreshToken);
            var affectedRows = await _dbContext.SaveChangesAsync();
            return affectedRows > 0;
        }
        public async Task DeleteExpiredTokens()
        {
            DbSet<RefreshToken> refreshTokenDbContex = _dbContext.Set<RefreshToken>();

            List<RefreshToken> listToBeDeleted = await refreshTokenDbContex.Where(x => x.ExpiredIn < DateTime.UtcNow)
                .ToListAsync();

            refreshTokenDbContex.RemoveRange(listToBeDeleted);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string refreshToken)
        {
            var result = await _dbContext.Set<RefreshToken>()
                                 .Where(p => p.Token.ToString() == refreshToken)
                                 .FirstOrDefaultAsync();
            return result;
        }
    }
}
