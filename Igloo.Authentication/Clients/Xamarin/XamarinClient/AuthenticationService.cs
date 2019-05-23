using IdentityModel.OidcClient;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamarinClient
{
    /// <summary>
    /// Service working with account storage allows user to ligin, refresh token and logout.
    /// </summary>
    public class AuthenticationService
    {
        private readonly OidcClient _oidcClient;
        private readonly AccountStorageService _accountStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="oidcClient">The oidc client.</param>
        /// <param name="accountStorageService">The account storage service.</param>
        public AuthenticationService(
            OidcClient oidcClient,
            AccountStorageService accountStorageService)
        {
            _oidcClient = oidcClient;
            _accountStorageService = accountStorageService;
        }

        /// <summary>
        /// Refreshes and stores the new token in account storage.
        /// Returns account you can use to access api.
        /// </summary>
        /// <returns>Account.</returns>
        public async Task<Account> RefreshTokenAsync()
        {
            var account = await _accountStorageService.GetAccountAsync();
            if (account != null)
            {
                var refreshTokenResult = await _oidcClient.RefreshTokenAsync(account.RefreshToken);
                if (!refreshTokenResult.IsError)
                {
                    var newAccount = new Account
                    {
                        AccessToken = refreshTokenResult.AccessToken,
                        RefreshToken = refreshTokenResult.RefreshToken,
                        Expiration = refreshTokenResult.AccessTokenExpiration
                    };
                    await _accountStorageService.SetAccountAsync(newAccount);
                    return newAccount;
                }
            }
            return null;
        }

        /// <summary>
        /// Logins user and stores token in account storage.
        /// Returns account you can use to access api.
        /// </summary>
        /// <returns>Account.</returns>
        public async Task<Account> LoginAsync()
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest());
            if (!loginResult.IsError)
            {
                var newAccount = new Account
                {
                    AccessToken = loginResult.AccessToken,
                    RefreshToken = loginResult.RefreshToken,
                    Expiration = loginResult.AccessTokenExpiration
                };
                await _accountStorageService.SetAccountAsync(newAccount);
                return newAccount;
            }
            return null;
        }

        /// <summary>
        /// Logouts and clears account storage.
        /// </summary>
        public void LogoutAsync()
        {
            _accountStorageService.ClearAccount();
            //await _oidcClient.LogoutAsync();
        }
    }
}
