using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Domain.Entities;
using ChargeHubAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChargeHubAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChargeHubDbContext _context;

    public UserRepository(ChargeHubDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
        _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public Task<User?> GetByUserIdAsync(string userId, CancellationToken cancellationToken) =>
        _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken) =>
        _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

    public Task<User?> GetByIdentecationAsync(string identecation, CancellationToken cancellationToken) =>
        _context.Users
            .FirstOrDefaultAsync(u => u.Identecation == identecation, cancellationToken);

    public Task<User?> GetNameByIdentecationAsync(string identecation, CancellationToken cancellationToken) =>
    _context.Users
     .FirstOrDefaultAsync(u => u.Identecation != null &&
                               EF.Functions.Like(u.Identecation.Trim(), identecation.Trim()),
                            cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([userId], cancellationToken);
        if (user is null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}



