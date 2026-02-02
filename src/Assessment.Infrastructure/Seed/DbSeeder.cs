using Assessment.Infrastructure.Persistence;
using Assessment.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assessment.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, string fakeProviderBaseUrl)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        // Seed Providers
        if (!await db.PaymentProviders.AnyAsync())
        {
            db.PaymentProviders.Add(new PaymentProviderEntity
            {
                Id = Guid.NewGuid(),
                Name = "FakeProvider",
                IsActive = true,
                EndpointUrl = fakeProviderBaseUrl
            });

            await db.SaveChangesAsync();
        }
    }
}
