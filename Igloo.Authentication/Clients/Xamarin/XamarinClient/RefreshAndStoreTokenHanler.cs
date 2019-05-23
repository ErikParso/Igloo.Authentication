using IdentityModel.Client;
using System;
using System.Net.Http;

namespace XamarinClient
{
    /// <summary>
    /// Extension of <see cref="RefreshTokenDelegatingHandler"/> stores refreshed tokens into
    /// secure store using <see cref="AccountStorageService"/>.
    /// </summary>
    /// <seealso cref="RefreshTokenDelegatingHandler" />
    public class RefreshAndStoreTokenHandler : RefreshTokenDelegatingHandler
    {
        private readonly AccountStorageService _accountStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshAndStoreTokenHandler"/> class.
        /// </summary>
        /// <param name="accountStorageService">The account storage service.</param>
        /// <param name="account">The account.</param>
        /// <param name="tokenEndpoint">The token endpoint.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        public RefreshAndStoreTokenHandler(
            AccountStorageService accountStorageService,
            Account account,
            string tokenEndpoint,
            string clientId,
            string clientSecret = null)
            : base(tokenEndpoint, clientId, clientSecret ?? string.Empty, account.RefreshToken, account.AccessToken, new HttpClientHandler())
        {
            _accountStorageService = accountStorageService;
            TokenRefreshed += RefreshTokenDelegatingHandler_TokenRefreshed;
        }

        private async void RefreshTokenDelegatingHandler_TokenRefreshed(object sender, TokenRefreshedEventArgs e)
            => await _accountStorageService.SetAccountAsync(new Account()
            {
                AccessToken = e.AccessToken,
                RefreshToken = e.RefreshToken,
                Expiration = DateTime.Now.AddSeconds(e.ExpiresIn),
            });
    }
}
