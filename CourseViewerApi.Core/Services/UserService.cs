using AutoMapper;
using CourseViewerApi.Common.DTO;
using CourseViewerApi.Core.Interfaces;
using CourseViewerApi.DataAccess.Entities;
using CourseViewerApi.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseViewerApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository repository,
            IConfiguration configuration,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserTokenDto>> LoginUserAsync(LoginDto loginDto)
        {
            var result = new ServiceResult<UserTokenDto>();

            try
            {
                User? user = await _repository.GetByEmailAsync(loginDto.Email);
                if (user is null)
                {
                    result.Success = false;
                    result.Errors.Add("User was not found!");
                }
                else
                {
                    bool isUserValid = await _repository.ValidatePasswordAsync(user, loginDto.Password);
                    if (!isUserValid)
                    {
                        result.Success = false;
                        result.Errors.Add("Wrong password!");
                    }
                    else
                    {
                        result.ResultValue = new UserTokenDto { Jwt = await GenerateTokenAsync(user) };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while executing the service");
                result.Success = false;
                result.Errors.Add("Failed to log in!");
            }
            
            return result;
        }

        public async Task<ServiceResult<UserTokenDto>> RegisterUserAsync(RegisterDto dto)
        {
            var result = new ServiceResult<UserTokenDto>();

            try
            {
                var user = new User
                {
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Type = dto.Type,
                    UserName = dto.Email,
                    Name = dto.Name
                };
                bool added = await _repository.AddAsync(user, dto.Password);
                if (added)
                {
                    result.ResultValue = new UserTokenDto { Jwt = await GenerateTokenAsync(user) };
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("Failed to register user!");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while executing the service");
                result.Success = false;
                result.Errors.Add("Failed to register user!");
            }

            return result;
        }

        public async Task<bool> UserExistsAsync(string email, CancellationToken token)
        {
            bool exists = true;

            try
            {
                exists = await _repository.ExistsAsync(email, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while executing the service");
                token.ThrowIfCancellationRequested();
            }

            return exists;
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName)
            };
            var roles = await _repository.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiresIn"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
