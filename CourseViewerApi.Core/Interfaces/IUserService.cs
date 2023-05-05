using CourseViewerApi.Common.DTO;

namespace CourseViewerApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserTokenDto>> LoginUserAsync(LoginDto loginDto);
        Task<bool> UserExistsAsync(string email, CancellationToken token);
    }
}
