using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Infrastructure.Security
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
