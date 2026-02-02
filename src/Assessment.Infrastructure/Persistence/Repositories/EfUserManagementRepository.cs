using Assessment.Application.Abstractions;
using Assessment.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Infrastructure.Persistence.Repositories;

public class EfUserManagementRepository : IUserManagementRepository
{
    private readonly AppDbContext _db;
    public EfUserManagementRepository(AppDbContext db) => _db = db;

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        => _db.Users.AsNoTracking().AnyAsync(u => u.Email == email, ct);

    public async Task<Guid> CreateUserAsync(string? userName, string email, string passwordHash, CancellationToken ct)
    {
        var id = Guid.NewGuid();
        _db.Users.Add(new UserEntity
        {
            Id = id,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PasswordHash = passwordHash,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
        return id;
    }

    public async Task EnsureRoleExistsAsync(string roleName, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
        if (role == null)
        {
            _db.Roles.Add(new RoleEntity { Id = Guid.NewGuid(), Name = roleName });
        }
    }

    public async Task AssignRoleAsync(Guid userId, string roleName, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct)
                   ?? throw new InvalidOperationException("Role missing.");

        var exists = await _db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == role.Id, ct);
        if (!exists)
        {
            _db.UserRoles.Add(new UserRoleEntity { UserId = userId, RoleId = role.Id });
        }
    }

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}

