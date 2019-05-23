using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Xamarin.Forms;
using XamarinClient;

namespace XamarinFormsDemo
{
    public partial class MainPage : ContentPage
    {
        private const string apiUrl = "https://testmobileservice.azurewebsites.net";
        private const string identityServerUrl = "https://iglooidentityserver.azurewebsites.net";
        private const string clientId = "xamarin-client";

        AuthenticationService _authenticationService;
        AccountStorageService _accountStorageService;
        HttpClient _client;

        public MainPage()
        {
            InitializeComponent();

            Login.Clicked += Login_Clicked;
            CallApi.Clicked += CallApi_Clicked;
            Logout.Clicked += Logout_Clicked;

            var browser = DependencyService.Get<IBrowser>();

            var options = new OidcClientOptions
            {
                Authority = identityServerUrl,
                ClientId = clientId,
                Scope = "openid values-api offline_access",
                RedirectUri = "xamarinformsclients://callback",
                Browser = browser,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
            };

            var client = new OidcClient(options);

            _accountStorageService = new AccountStorageService("xamarin-client-storage");
            _authenticationService = new AuthenticationService(client, _accountStorageService);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            var account = await _authenticationService.RefreshTokenAsync() ?? await _authenticationService.LoginAsync();

            if (account == null)
            {
                _client = new HttpClient(new HttpClientHandler());
                OutputText.Text = "Login Failed";
            }
            else
            {
                _client = new HttpClient(new RefreshAndStoreTokenHandler(
                    _accountStorageService, account, identityServerUrl + "/connect/token", clientId));
                OutputText.Text = "Login successfull";
            }

            _client.BaseAddress = new Uri(apiUrl);
        }

        private async void CallApi_Clicked(object sender, EventArgs e)
        {
            var result = await _client.GetAsync("api/values");
            if (result.IsSuccessStatusCode)
            {
                OutputText.Text = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
            }
            else
            {
                OutputText.Text = result.ReasonPhrase;
            }
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            _authenticationService.LogoutAsync();
            OutputText.Text = "Logget out";
        }
    }
}
