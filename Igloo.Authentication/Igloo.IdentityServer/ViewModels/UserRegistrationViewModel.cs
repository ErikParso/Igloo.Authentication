using System.ComponentModel.DataAnnotations;

namespace Igloo.IdentityServer.ViewModels
{
    public class UserRegistrationViewModel
    {
        public UserRegistrationViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public UserRegistrationViewModel()
        {

        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(256)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [MaxLength(256)]
        public string WebSite { get; set; }

        public string ReturnUrl { get; set; }
    }
}
