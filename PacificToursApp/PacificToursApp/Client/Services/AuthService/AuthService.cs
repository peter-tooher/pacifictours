namespace PacificToursApp.Client.Services.AuthService
{
    // The AuthService class implements the IAuthService interface
    public class AuthService : IAuthService
    {
        // A private readonly HttpClient field to make HTTP requests
        private readonly HttpClient _http;

        // The constructor takes a HttpClient and assigns it to the _http field
        public AuthService(HttpClient http)
        {
            _http = http;
        }

        // An asynchronous method that takes a UserChangePassword request and returns a ServiceResponse of type bool
        // It makes a POST request to the "api/auth/change-password" endpoint with the new password as the body
        // It then deserializes the response to a ServiceResponse of type bool
        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", request.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        // An asynchronous method that takes a UserLogin request and returns a ServiceResponse of type string
        // It makes a POST request to the "api/auth/login" endpoint with the UserLogin request as the body
        // It then deserializes the response to a ServiceResponse of type string
        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        // An asynchronous method that takes a UserRegister request and returns a ServiceResponse of type int
        // It makes a POST request to the "api/auth/register" endpoint with the UserRegister request as the body
        // It then deserializes the response to a ServiceResponse of type int
        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}