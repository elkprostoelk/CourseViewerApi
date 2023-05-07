using CourseViewerApi.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace CourseViewerApi.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(User user, string password);
        Task<bool> ExistsAsync(string email, CancellationToken token);
        Task<User?> GetByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(User user);
        Task<bool> ValidatePasswordAsync(User user, string password);
    }
}
