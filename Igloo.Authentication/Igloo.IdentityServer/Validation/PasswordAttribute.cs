using System.ComponentModel.DataAnnotations;

namespace Igloo.IdentityServer.Validation
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
          => ((string)value).ValidatePassword()
                ? ValidationResult.Success
                : new ValidationResult(PasswordSettings.GetInvalidPasswordMessage());
    }
}
