using CourseViewerApi.Core.Interfaces;
using FluentValidation;

namespace CourseViewerApi.Web.Extensions
{
    public static class FLuentValidationExtensions
    {
        public static IRuleBuilder<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Matches(@"^\+\d+$")
                .WithMessage("Phone number must be in the following format: +380112223333");

        public static IRuleBuilder<T, string> CourseViewerPassword<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Length(8, 20)
                .Matches(@"^[0-9A-Za-z]+$")
                .WithMessage("Password should contain at least 1 digit, 1 uppercase letter and 1 lowercase letter without any special symbols!");
        
        public static IRuleBuilder<T, string> UniqueUser<T>(this IRuleBuilder<T, string> ruleBuilder, IUserService userService) =>
            ruleBuilder.MustAsync(async (string email, CancellationToken token) =>
                           !await userService.UserExistsAsync(email, token))
                .WithMessage(dto => $"User already exists!");
    }
}
