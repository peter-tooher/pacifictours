namespace PacificToursApp.Client.Services.AuthService
{
    // The IAuthService interface defines the contract for the AuthService
    public interface IAuthService
    {
        // An asynchronous method that takes a UserRegister request and returns a ServiceResponse of type int
        // This method is used to register a new user
        Task<ServiceResponse<int>> Register(UserRegister request);

        // An asynchronous method that takes a UserLogin request and returns a ServiceResponse of type string
        // This method is used to log in a user
        Task<ServiceResponse<string>> Login(UserLogin request);

        // An asynchronous method that takes a UserChangePassword request and returns a ServiceResponse of type bool
        // This method is used to change a user's password
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    }
}