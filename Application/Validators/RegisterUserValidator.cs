using Application.Commands.RegisterUserRequest;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator(IConfiguration config)
        {
            var policy = config.GetSection("PasswordPolicy");
            
            int minLength = policy.GetValue<int>("MinLength");
            int minSpecial = policy.GetValue<int>("MinSpecialChars");
            string specialChars = policy.GetValue<string>("SpecialChars");

            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.username)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.fullname)
                .NotEmpty();

            RuleFor(x => x.password)
                .NotEmpty()
                .MinimumLength(minLength)
                .Must(p => p.Count(c => specialChars.Contains(c)) >= minSpecial)
                .WithMessage($"Password must contain at least {minSpecial} special characters: {specialChars}");
        }
    }
}
