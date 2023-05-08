using CourseViewerApi.Common.DTO;
using CourseViewerApi.Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace CourseViewerApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IValidator<RegisterDto> _validator;

        public UserController(
            IUserService service,
            IValidator<RegisterDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (User.Identity.IsAuthenticated
                && User.IsInRole("student"))
            {
                return StatusCode(403, "Students are not allowed to register users!");
            }

            if (dto.Type == Common.Enums.UserType.Admin
                && !(User.Identity.IsAuthenticated
                && User.IsInRole("admin")))
            {
                return StatusCode(403, "Non-authorized and non-admin users cannot register admins!");
            }

            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            ServiceResult<UserTokenDto> result = await _service.RegisterUserAsync(dto);
            if (result.Success)
            {
                return User.Identity.IsAuthenticated
                    ? StatusCode(201, result.ResultValue)
                    : StatusCode(201);
            }
            return BadRequest(result.Errors);
        }
    }
}
