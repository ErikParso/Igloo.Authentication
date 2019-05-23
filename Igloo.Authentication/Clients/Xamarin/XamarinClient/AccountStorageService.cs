using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinClient
{
    /// <summary>
    /// Stores account object in secure storage.
    /// </summary>
    public class AccountStorageService
    {
        private readonly string _key;
        private Account _currentAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountStorageService"/> class.
        /// </summary>
        /// <param name="storageKey">The storage key.</param>
        public AccountStorageService(string storageKey)
        {
            _key = storageKey;
        }

        /// <summary>
        /// Loads the account from secure storage.
        /// </summary>
        public async Task<Account> GetAccountAsync()
        {
            if (_currentAccount == null)
            {
                var accountString = await SecureStorage.GetAsync(_key);
                _currentAccount = string.IsNullOrWhiteSpace(accountString)
                    ? null : JsonConvert.DeserializeObject<Account>(accountString);
            }
            return _currentAccount;
        }

        /// <summary>
        /// Stores account in a secure storage.
        /// </summary>
        /// <param name="account">The account.</param>
        public async Task SetAccountAsync(Account account)
        {
            _currentAccount = account;
            var accountString = JsonConvert.SerializeObject(account);
            await SecureStorage.SetAsync(_key, accountString);
        }

        /// <summary>
        /// Removes account from secure storage.
        /// </summary>
        public void ClearAccount()
        {
            _currentAccount = null;
            SecureStorage.Remove(_key);
        }
    }
}
