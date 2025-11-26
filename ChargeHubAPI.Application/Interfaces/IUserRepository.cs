using ChargeHubAPI.Domain.Entities;

namespace ChargeHubAPI.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdentecationAsync(string identecation, CancellationToken cancellationToken);
    Task<User?> GetNameByIdentecationAsync(string identecation, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string userId, CancellationToken cancellationToken);
}