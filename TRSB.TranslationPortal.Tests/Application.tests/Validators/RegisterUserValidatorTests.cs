using Application.Commands.RegisterUserRequest;
using Application.Validators;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Configuration;

namespace TRSB.TranslationPortal.Tests.Application.tests.Validators
{
    public class RegisterUserValidatorTests
    {
        private RegisterUserValidator CreateValidator(
        int minLength = 8,
        int minSpecial = 2,
        string specialChars = "!@#$%^&*")
        {
            var inMemorySettings = new Dictionary<string, string>
        {
            { "PasswordPolicy:MinLength", minLength.ToString() },
            { "PasswordPolicy:MinSpecialChars", minSpecial.ToString() },
            { "PasswordPolicy:SpecialChars", specialChars }
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new RegisterUserValidator(config);
        }

        private RegisterUserCommand ValidCommand => new RegisterUserCommand
        (
             "caluchi",
             "Carolina Pelacini",
             "caro@mail.com",
             "Pass!!word"
        );

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var validator = CreateValidator();
            var result = validator.TestValidate(ValidCommand);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Email_Is_Empty()
        {
            var validator = CreateValidator();
            var cmd = new RegisterUserCommand("caluchi", "Carolina Pelacini", "", "Pass!!word");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.email);
        }

        [Fact]
        public void Should_Fail_When_Email_Is_Invalid()
        {
            var validator = CreateValidator();
            var cmd = new RegisterUserCommand("caluchi", "Carolina Pelacini", "not-an-email", "Pass!!word");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.email);
        }

        [Fact]
        public void Should_Fail_When_Username_Too_Short()
        {
            var validator = CreateValidator();
            var cmd = new RegisterUserCommand("ab", "Carolina Pelacini", "caro@mail.com", "Pass!!word");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.username);
        }

        [Fact]
        public void Should_Fail_When_Fullname_Is_Empty()
        {
            var validator = CreateValidator();
            var cmd = new RegisterUserCommand("caluchi", "", "caro@mail.com", "Pass!!word");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.fullname);
        }

        [Fact]
        public void Should_Fail_When_Password_Too_Short()
        {
            var validator = CreateValidator(minLength: 10);
            var cmd = new RegisterUserCommand("caluchi", "Carolina Pelacini", "caro@mail.com", "Short!");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.password);
        }

        [Fact]
        public void Should_Fail_When_Password_Missing_Special_Chars()
        {
            var validator = CreateValidator(minSpecial: 2, specialChars: "!@#$");
            var cmd = new RegisterUserCommand("caluchi", "Carolina Pelacini", "caro@mail.com", "Password1!");

            var result = validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.password);
        }

        [Fact]
        public void Should_Pass_When_Password_Meets_Special_Char_Requirement()
        {
            var validator = CreateValidator(minSpecial: 2, specialChars: "!@#$");
            var cmd = new RegisterUserCommand("caluchi", "Carolina Pelacini", "caro@mail.com", "Good!!Pass");

            var result = validator.TestValidate(cmd);
            result.ShouldNotHaveValidationErrorFor(x => x.password);
        }
    }
}
