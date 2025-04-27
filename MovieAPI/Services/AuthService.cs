using MovieAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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

        // Register a new user
        public async Task<string> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Email == email);
            if (userExists)
                return "Email is already registered.";

            // Hash password
            var passwordHash = HashPassword(password);

            var user = new User
            {
                Id = GetNextUserId(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,  
                Status = "Active",
                Rank = 1
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return "User registered successfully!";
        }

        // Login user
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return "User not found.";

            if (!VerifyPassword(user.PasswordHash, password))
                return "Invalid password.";

            return "Login successful!";
        }

        // Method to hash password (you can use more advanced methods such as PBKDF2, bcrypt, etc.)
        private string HashPassword(string password)
        {
            var salt = new byte[16];
            new Random().NextBytes(salt);

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash;
        }

        // Method to verify if password matches the stored hash
        private bool VerifyPassword(string storedHash, string enteredPassword)
        {
            return storedHash == HashPassword(enteredPassword);
        }

        private int GetNextUserId()
        {
            var lastUser = _dbContext.Users.OrderByDescending(u => u.Id).FirstOrDefault();
            return lastUser == null ? 1 : lastUser.Id + 1;
        }
    }
}
