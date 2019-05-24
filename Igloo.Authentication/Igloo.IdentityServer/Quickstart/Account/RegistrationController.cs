using IdentityModel;
using Igloo.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Quickstart.UI
{
    [Route("api/account/[Controller]")]
    public class RegistrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RegistrationController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("register")]
        public ViewResult Register([FromQuery]string returnUrl)
            => View("Views/Account/Registration.cshtml", new RegistrationViewModel(returnUrl));

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegistrationViewModel viewModel)
        {
            var user = userManager.FindByNameAsync(viewModel.Name).Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = viewModel.Name
                };
                var result = userManager.CreateAsync(user, viewModel.Password).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(user, new Claim[]{
                        new Claim(JwtClaimTypes.Name, viewModel.UserName),
                        new Claim(JwtClaimTypes.GivenName, viewModel.UserName.Split(' ')[0]),
                        new Claim(JwtClaimTypes.FamilyName, viewModel.UserName.Split(' ')[1]),
                        new Claim(JwtClaimTypes.Email, viewModel.Email ?? string.Empty),
                        new Claim(JwtClaimTypes.WebSite, viewModel.WebSite ?? string.Empty),
                    }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                return RedirectToAction("login", "account", new { returnUrl = viewModel.ReturnUrl });
            }
            else
            {
                return Conflict("User already exists.");
            }
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult Cancel(RegistrationViewModel viewModel)
            => RedirectToAction("login", "account", new { returnUrl = viewModel.ReturnUrl });
    }
}