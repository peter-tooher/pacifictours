namespace PacificToursApp.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<bool> UserExists(string username);
    }
}
