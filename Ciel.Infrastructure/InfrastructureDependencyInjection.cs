using Ciel.Infrastructure.Core.Contracts.Repositories;
using Ciel.Infrastructure.Core.Contracts.Services;
using Ciel.Infrastructure.Core.Models;
using Ciel.Infrastructure.Helpers;
using Ciel.Infrastructure.Repositories;
using Ciel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ciel.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddIdentity();

        services.AddRepositories();

        services.AddServices();

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            opt => opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));

        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(DeletableEntityRepository<>));
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClaimService, ClaimService>();

        services.AddScoped<IJwtService, JwtService>();
    }
}