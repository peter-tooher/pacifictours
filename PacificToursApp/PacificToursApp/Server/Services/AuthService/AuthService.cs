using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PacificToursApp.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(username.ToLower()));
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
            else
            {
                response.Data = CreateToken(user);
            }
            
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            if (await UserExists(request.UserName))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

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

            return new ServiceResponse<int>
            {
                Data = user.UserId,
                Message = "User created successfully."
            };
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(user => user.UserName.ToLower()
            .Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int UserId, string newPassword)
        {
            var user = await _context.Users.FindAsync(UserId);

            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }  
            
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password changed successfully."
            };
        }
        
    }
}