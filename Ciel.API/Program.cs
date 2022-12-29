using Ciel.API.Core;
using Ciel.API.Extensions;
using Ciel.API.Filters;
using Ciel.API.Middleware;
using Ciel.API.Models;
using Ciel.Infrastructure;
using Ciel.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config => config.Filters.Add(typeof(ValidateModelAttribute)));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPersistence();

// Configuration
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(nameof(AppConfig)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed data on application startup
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsSqlServer())
    {
        dbContext.Database.Migrate();
    }
    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// Middleware
app.UseMiddleware<TransactionMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();