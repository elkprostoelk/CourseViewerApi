﻿using CourseViewerApi.Common.DTO;
using CourseViewerApi.Core.Interfaces;
using CourseViewerApi.Web.Extensions;
using FluentValidation;

namespace CourseViewerApi.Web.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator(IUserService userService)
        {
            RuleFor(dto => dto.Email)
                .EmailAddress()
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (string email, CancellationToken token) =>
                           !await userService.UserExistsAsync(email, token))
                .WithMessage(dto => $"User {dto.Email} does not exist!");

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(8, 20)
                .CourseViewerPassword();
        }
    }
}
