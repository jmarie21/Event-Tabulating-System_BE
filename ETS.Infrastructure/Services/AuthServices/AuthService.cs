using ETS.Application.Common.Interfaces;
using ETS.Application.Users.DTOs;
using ETS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ETS.Infrastructure.Services.AuthServices
{
    public class AuthService(IAppDbContext context, IConfiguration configuration) : IAuthService
    {
        private readonly IAppDbContext _context = context;

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, password) == PasswordVerificationResult.Failed)
                return null!;

            return GenerateToken(user);

        }

        public async Task<UserDto?> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return null!;

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = hashedPassword,
                UserRole = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
            };
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
