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
                return "Invalid username or password.";

            if (!VerifyPassword(password, user.PasswordHash))
                return "Invalid password.";

            if (!string.Equals(user.Status, "active", StringComparison.OrdinalIgnoreCase))
                return "User account has been blocked";

            var token = GenerateJwtToken(user);

            return $"Login successful! Token: {token}";
        }

        private string GenerateJwtToken(User user)
        { 
            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");
             
            var expiresIn = DateTime.UtcNow.AddHours(1);
             
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
             
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
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
             
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
             
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
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<string> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return "User not found.";

            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return "Current password is incorrect.";

            user.PasswordHash = HashPasswordWithSalt(newPassword);
            await _dbContext.SaveChangesAsync();

            return "Password changed successfully.";
        }
        public async Task<string> RequestRoleChangeAsync(int userId, string requestedRole)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return "User not found.";
             
            var requestedRoleSerbian = RoleTranslationHelper.ToSerbian(requestedRole);

            if (user.Role.ToString().ToLower() == requestedRoleSerbian.ToLower())
                return "You already have this role.";

            var request = new Request
            {
                UserId = userId,
                RequestedRole = requestedRoleSerbian,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Requests.AddAsync(request);
            await _dbContext.SaveChangesAsync();

            return $"Role change request to '{requestedRole}' submitted for review.";
        }

        public async Task<List<Request>> GetAllRoleRequestsAsync()
        {
            return await _dbContext.Requests
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<string> VerifyRoleRequestAsync(int requestId)
        {
            var request = await _dbContext.Requests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                return "Request not found.";

            var user = request.User;
            var oldRole = user.RoleString.ToLower();
            var newRole = request.RequestedRole.ToLower();
             
            switch (oldRole)
            {
                case "obican_korisnik":
                    var regUser = await _dbContext.RegularUsers.FindAsync(user.Id);
                    if (regUser != null) _dbContext.RegularUsers.Remove(regUser);
                    break;
                case "administrator":
                    var admin = await _dbContext.Administrators.FindAsync(user.Id);
                    if (admin != null) _dbContext.Administrators.Remove(admin);
                    break;
                case "urednik_sadrzaja":
                    var editor = await _dbContext.Editors.FindAsync(user.Id);
                    if (editor != null) _dbContext.Editors.Remove(editor);
                    break;
                case "moderator":
                    var mod = await _dbContext.Moderators.FindAsync(user.Id);
                    if (mod != null) _dbContext.Moderators.Remove(mod);
                    break;
                case "filmski_kriticar":
                    var critic = await _dbContext.Critics.FindAsync(user.Id);
                    if (critic != null) _dbContext.Critics.Remove(critic);
                    break;
            }
             
            switch (newRole)
            {
                case "obican_korisnik":
                    await _dbContext.RegularUsers.AddAsync(new RegularUser { Id = user.Id });
                    break;
                case "administrator":
                    await _dbContext.Administrators.AddAsync(new Administrator { Id = user.Id });
                    break;
                case "urednik_sadrzaja":
                    await _dbContext.Editors.AddAsync(new Editor { Id = user.Id });
                    break;
                case "moderator":
                    await _dbContext.Moderators.AddAsync(new Moderator { Id = user.Id });
                    break;
                case "filmski_kriticar":
                    await _dbContext.Critics.AddAsync(new Critic { Id = user.Id });
                    break;
                default:
                    return "Invalid role.";
            }
             
            user.RoleString = request.RequestedRole;
            request.Status = "approved";

            await _dbContext.SaveChangesAsync();

            return $"User {user.Email} role updated to {request.RequestedRole}.";
        }


        public async Task<string> DeclineRoleRequestAsync(int requestId)
        {
            var request = await _dbContext.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null) return "Request not found.";

            request.Status = "declined";
            await _dbContext.SaveChangesAsync();

            return "Request declined.";
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<List<User>> GetAllUsersExceptAsync(int excludedUserId)
        {
            return await _dbContext.Users
                .Where(u => u.Id != excludedUserId)
                .ToListAsync();
        }

        public async Task<bool> FollowUserAsync(int followerId, int followeeId)
        {
            if (await _dbContext.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId))
                return false;

            var follow = new Follow { FollowerId = followerId, FolloweeId = followeeId };
            await _dbContext.Follows.AddAsync(follow);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        { 
            var followerIds = await _dbContext.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FolloweeId)
                .ToListAsync();

            var followers = await _dbContext.Users
                .Where(u => followerIds.Contains(u.Id))
                .ToListAsync();

            return followers;
        }

        public async Task<string> BlockUserAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return "User not found.";

            if (string.Equals(user.Status, "blocked", StringComparison.OrdinalIgnoreCase))
                return "User is already blocked.";

            user.Status = "blocked";
            await _dbContext.SaveChangesAsync();

            return $"User {user.Email} has been blocked.";
        }


    }
}
