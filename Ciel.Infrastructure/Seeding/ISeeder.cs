namespace Ciel.Infrastructure.Seeding;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}