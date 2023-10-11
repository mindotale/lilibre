using Lilibre.Application;
using Lilibre.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Api.Configurations;

public static class PersistenceConfigurationExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                var connectionString = configuration.GetValue<string>("ConnectionString");
                options.UseSqlServer(connectionString);
            });
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Book, int>, Repository<Book, int>>();
        services.AddScoped<IRepository<Author, int>, Repository<Author, int>>();
        services.AddScoped<IRepository<Genre, int>, Repository<Genre, int>>();

        return services;
    }
}
