using CourseViewerApi.Common.DTO;
using CourseViewerApi.Core.Interfaces;
using CourseViewerApi.Web.Extensions;
using FluentValidation;

namespace CourseViewerApi.Web.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator(IUserService userService)
        {
            RuleFor(dto => dto.Email)
                .EmailAddress()
                .NotEmpty()
                .MaximumLength(100)
                .UniqueUser(userService);

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .CourseViewerPassword();

            RuleFor(dto => dto.PhoneNumber)
                .PhoneNumber();

            RuleFor(dto => dto.Name)
                .MaximumLength(50);

            RuleFor(dto => dto.Type)
                .NotNull()
                .IsInEnum();
        }
    }
}
