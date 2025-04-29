using MovieAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using MovieAPI.Models.User;

namespace MovieAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Email == email);
            if (userExists)
                return "Email is already registered.";

            var passwordHashWithSalt = HashPasswordWithSalt(password);

            var user = new User
            {
                Id = GetNextUserId(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHashWithSalt,
                Status = "active",
                Rank = 1,
                Role = UserRole.obican_korisnik
            };
            var regularUser = new RegularUser
            {
                Id = user.Id
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.RegularUsers.AddAsync(regularUser);
            await _dbContext.SaveChangesAsync();

            return "User registered successfully!";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return "User not found.";

            if (!VerifyPassword(password, user.PasswordHash))
                return "Invalid password.";

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return $"Login successful! Token: {token}";
        }

        private string GenerateJwtToken(User user)
        {
            // Read the secret key from the configuration (appsettings.json)
            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");

            // Define token expiration time (e.g., 1 hour)
            var expiresIn = DateTime.UtcNow.AddHours(1);

            // Create security key from the secret key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("id", user.Id.ToString()),
                    new System.Security.Claims.Claim("email", user.Email),
                    new System.Security.Claims.Claim("role", user.Role.ToString())
                }),
                Expires = expiresIn,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPasswordWithSalt(string password)
        {
            // Generate salt
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // Derive hash
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Combine salt + hash (as base64)
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private bool VerifyPassword(string enteredPassword, string storedHashWithSalt)
        {
            var parts = storedHashWithSalt.Split(':');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            var enteredHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return enteredHash == storedHash;
        }

        private int GetNextUserId()
        {
            var lastUser = _dbContext.Users.OrderByDescending(u => u.Id).FirstOrDefault();
            return lastUser == null ? 1 : lastUser.Id + 1;
        }
    }
}
