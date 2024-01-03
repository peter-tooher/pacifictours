namespace PacificToursApp.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        public AuthService (DataContext context)
        {
            _context = context;
        }
        public Task<ServiceResponse<int>> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(user => user.UserName.ToLower()
            .Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }
    }
}