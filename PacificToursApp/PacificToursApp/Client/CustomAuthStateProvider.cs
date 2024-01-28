using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

namespace PacificToursApp.Client
{
    // CustomAuthStateProvider inherits from AuthenticationStateProvider
    // It provides the authentication state for the application
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        // Declare private fields for the local storage service and the HTTP client
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        // Inject the local storage service and the HTTP client into the provider via the constructor
        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }

        // Override the GetAuthenticationStateAsync method to provide the authentication state
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Get the authentication token from local storage
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            // Initialize a new ClaimsIdentity and clear the Authorization header of the HTTP client
            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            // If the authentication token is not null or empty, try to parse the claims from it
            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    // Parse the claims from the JWT and set the Authorization header of the HTTP client
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken.Replace("\"", ""));
                }
                catch
                {
                    // If parsing the claims fails, remove the authentication token from local storage and initialize a new ClaimsIdentity
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            // Create a new ClaimsPrincipal with the ClaimsIdentity
            var user = new ClaimsPrincipal(identity);

            // Create a new AuthenticationState with the ClaimsPrincipal
            var state = new AuthenticationState(user);

            // Notify the application that the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            // Return the authentication state
            return state;
        }

        // Method to parse a Base64 string without padding
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        // Method to parse the claims from a JWT
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            // Split the JWT into parts and get the payload
            var payload = jwt.Split('.')[1];
            // Parse the Base64 payload into bytes
            var jsonByes = ParseBase64WithoutPadding(payload);
            // Deserialize the bytes into a dictionary
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonByes);
            // Create a new claim for each key-value pair in the dictionary
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            // Return the claims
            return claims;
        }
    }
}