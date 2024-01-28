namespace PacificToursApp.Server.Services.AuthService
{
    // The IAuthService interface defines the contract for the authentication service.
    public interface IAuthService
    {
        // The Register method signature. This method should take a UserRegister request and return a ServiceResponse containing the ID of the newly registered user.
        Task<ServiceResponse<int>> Register(UserRegister request);

        // The UserExists method signature. This method should take a username and return a boolean indicating whether a user with that username exists.
        Task<bool> UserExists(string username);

        // The Login method signature. This method should take a username and password and return a ServiceResponse containing a JWT (JSON Web Token) if the login is successful.
        Task<ServiceResponse<string>> Login(string username, string password);

        // The ChangePassword method signature. This method should take a UserId and a new password and return a ServiceResponse indicating whether the password change was successful.
        Task<ServiceResponse<bool>> ChangePassword(int UserId, string newPassword);
    }
}
