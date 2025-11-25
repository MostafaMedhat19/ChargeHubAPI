using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChargeHubAPI.Infrastructure.Persistence;

public class ChargeHubDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChargeHubDbContext>
{
    public ChargeHubDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChargeHubDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChargeHubDb;Trusted_Connection=True;TrustServerCertificate=True");
        return new ChargeHubDbContext(optionsBuilder.Options);
    }
}




