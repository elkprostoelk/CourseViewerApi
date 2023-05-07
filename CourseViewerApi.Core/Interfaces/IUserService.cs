using CourseViewerApi.Common.DTO;

namespace CourseViewerApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserTokenDto>> LoginUserAsync(LoginDto loginDto);
        Task<ServiceResult<UserTokenDto>> RegisterUserAsync(RegisterDto dto);
        Task<bool> UserExistsAsync(string email, CancellationToken token);
    }
}
