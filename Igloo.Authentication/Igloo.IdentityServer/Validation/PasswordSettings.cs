using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text;

namespace Igloo.IdentityServer.Validation
{
    public static class PasswordSettings
    {
        private const int Lenght = 8;
        private const bool UpperCase = false;
        private const bool Numeric = true;
        private const bool NonAlphaNumeric = false;

        public static void SetPasswordOptions(this IdentityOptions identityOptions)
        {
            identityOptions.Password.RequiredLength = Lenght;
            identityOptions.Password.RequireUppercase = UpperCase;
            identityOptions.Password.RequireDigit = Numeric;
            identityOptions.Password.RequireNonAlphanumeric = NonAlphaNumeric;
        }

        public static string GetInvalidPasswordMessage()
        {
            var sb = new StringBuilder();
            sb.Append($"Password must contain at least {Lenght} characters");
            sb.Append(UpperCase ? ", 1 uppercase" : string.Empty);
            sb.Append(Numeric ? ", 1 numeric" : string.Empty);
            sb.Append(NonAlphaNumeric ? ", 1 non alphanumeric" : string.Empty);
            sb.Append(".");
            return sb.ToString();
        }

        public static bool ValidatePassword(this string password)
            => !string.IsNullOrWhiteSpace(password) &&
               password.Length >= Lenght &&
               (!UpperCase || password.Any(char.IsUpper)) &&
               (!Numeric || password.Any(char.IsDigit)) &&
               (!NonAlphaNumeric || password.All(char.IsLetterOrDigit));
    }
}
