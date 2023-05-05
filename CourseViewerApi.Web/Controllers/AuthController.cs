using CourseViewerApi.Common.DTO;
using CourseViewerApi.Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace CourseViewerApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<LoginDto> _validator;

        public AuthController(
            IUserService userService,
            IValidator<LoginDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validationResult = await _validator.ValidateAsync(loginDto);
            if (validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            ServiceResult<UserTokenDto> result = await _userService.LoginUserAsync(loginDto);

            return result.Success
                ? Ok(result.ResultValue)
                : BadRequest(result.Errors);
        }
    }
}
