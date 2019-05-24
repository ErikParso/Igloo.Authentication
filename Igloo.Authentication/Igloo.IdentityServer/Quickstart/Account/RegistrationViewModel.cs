using Igloo.IdentityServer.Validation;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Quickstart.UI
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public RegistrationViewModel()
        {

        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, DataType(DataType.Password), Password]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(256), NameSurname]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }


        public string ReturnUrl { get; set; }
    }
}
