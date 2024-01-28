using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PacificToursApp.Server.Services.AuthService
{
    // The AuthService class provides methods for managing authentication. It implements the IAuthService interface.
    public class AuthService : IAuthService
    {
        // The _context field is a DataContext instance used to interact with the database.
        // The _configuration field is an IConfiguration instance used to access application configuration settings.
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        // The constructor takes a DataContext and IConfiguration instance and assigns them to the _context and _configuration fields.
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // The Login method authenticates a user.
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            // A new ServiceResponse<string> instance is created.
            var response = new ServiceResponse<string>();
            // The user with the specified username is retrieved from the database.
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(username.ToLower()));
            // If the user is not found or the password is incorrect, the Success property is set to false and a message is set.
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            // If the user is found and the password is correct, a JWT (JSON Web Token) is created and assigned to the Data property.
            else
            {
                response.Data = CreateToken(user);
            }

            // The ServiceResponse<string> instance is returned.
            return response;
        }

        // The Register method registers a new user.
        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            // If the user already exists, a ServiceResponse<int> instance with Success set to false and a message is returned.
            if (await UserExists(request.UserName))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            // A password hash and salt are created from the password.
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // A new User instance is created and added to the database.
            User user = new User
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserEmail = request.Email,
                PassportNumber = request.PassportNumber,
                ContactNumber = request.ContactNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // A ServiceResponse<int> instance with the ID of the newly registered user and a message is returned.
            return new ServiceResponse<int>
            {
                Data = user.UserId,
                Message = "User created successfully."
            };
        }

        // The UserExists method checks if a user exists.
        public async Task<bool> UserExists(string username)
        {
            // If a user with the specified username exists in the database, true is returned. Otherwise, false is returned.
            if (await _context.Users.AnyAsync(user => user.UserName.ToLower()
            .Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        // The CreatePasswordHash method creates a password hash and salt.
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // A new HMACSHA512 instance is created and used to compute the password hash and salt.
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // The VerifyPasswordHash method verifies a password hash.
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // A new HMACSHA512 instance is created with the password salt and used to compute a hash from the password.
            // If the computed hash equals the password hash, true is returned. Otherwise, false is returned.
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // The CreateToken method creates a JWT (JSON Web Token).
        private string CreateToken(User user)
        {
            // A list of claims is created.
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // A new SymmetricSecurityKey instance is created from the secret key in the configuration.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            // A new SigningCredentials instance is created with the key and the HMACSHA512 algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // A new JwtSecurityToken instance is created with the claims, an expiration date of 1 day from now, and the signing credentials.
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            // The JwtSecurityToken is serialized to a JWT string.
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // The JWT string is returned.
            return jwt;
        }

        // The ChangePassword method changes a user's password.
        public async Task<ServiceResponse<bool>> ChangePassword(int UserId, string newPassword)
        {
            // The user with the specified ID is retrieved from the database.
            var user = await _context.Users.FindAsync(UserId);

            // If the user is not found, a ServiceResponse<bool> instance with Success set to false and a message is returned.
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            // A password hash and salt are created from the new password.
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            // The user's password hash and salt are updated in the database.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            // A ServiceResponse<bool> instance with Data set to true and a message is returned.
            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password changed successfully."
            };
        }

    }
}