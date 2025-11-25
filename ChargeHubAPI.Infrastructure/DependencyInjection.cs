using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Infrastructure.Email;
using ChargeHubAPI.Infrastructure.Persistence;
using ChargeHubAPI.Infrastructure.Repositories;
using ChargeHubAPI.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChargeHubAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, JwtSettings jwtSettings)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var smtpSection = configuration.GetSection("Smtp");
        var smtpSettings = new SmtpSettings
        {
            Host = smtpSection["Host"] ?? "smtp.gmail.com",
            Port = smtpSection.GetValue("Port", 587),
            UseStartTls = smtpSection.GetValue("UseStartTls", true),
            FromName = smtpSection["FromName"] ?? "ChargeHub Wireless",
            FromEmail = smtpSection["FromEmail"] ?? smtpSection["UserName"] ?? string.Empty,
            UserName = smtpSection["UserName"] ?? string.Empty,
            Password = smtpSection["Password"] ?? string.Empty
        };

        services.AddSingleton<IOptions<SmtpSettings>>(Options.Create(smtpSettings));

        services.AddSingleton(jwtSettings);

        services.AddDbContext<ChargeHubDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<IEmailSender, MailKitEmailSender>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();

        return services;
    }
}



