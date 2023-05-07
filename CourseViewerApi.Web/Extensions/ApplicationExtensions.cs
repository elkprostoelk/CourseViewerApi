using CourseViewerApi.Core.Interfaces;
using CourseViewerApi.DataAccess;
using CourseViewerApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace CourseViewerApi.Web.Extensions
{
    public static class ApplicationExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app, IConfiguration configuration)
        {
            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<CourseViewerDbContext>();
            if (dbContext is not null)
            {
                await dbContext.Database.EnsureCreatedAsync();
                var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var logger = app.Logger;
                if (await userManager.FindByEmailAsync("admin@admin.com") is null)
                {
                    var registeredResult = await userService.RegisterUserAsync(new Common.DTO.RegisterDto
                    {
                        Password = configuration.GetValue<string>("AdminUserInfo:Password"),
                        Email = configuration.GetValue<string>("AdminUserInfo:Email"),
                        PhoneNumber = "+380000000000",
                        Type = Common.Enums.UserType.Admin,
                        Name = "Admin"
                    });
                    if (registeredResult.Success)
                    {
                        logger.LogInformation("Admin user was registered!");
                    }
                    else
                    {
                        logger.LogError("Admin user was not registered!");
                    }
                }
            }
        }
    }
}
