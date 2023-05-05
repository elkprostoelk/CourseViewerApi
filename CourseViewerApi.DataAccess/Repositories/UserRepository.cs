using CourseViewerApi.DataAccess.Entities;
using CourseViewerApi.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseViewerApi.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CourseViewerDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(
            CourseViewerDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> ExistsAsync(string email, CancellationToken token) =>
            await _userManager.Users.AnyAsync(u => u.Email == email, token);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<IList<string>> GetRolesAsync(User user) =>
            await _userManager.GetRolesAsync(user);

        public async Task<bool> ValidatePasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);
    }
}
