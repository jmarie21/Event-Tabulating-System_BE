using ETS.Application.Common.Interfaces;
using ETS.Application.Users.DTOs;
using ETS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Infrastructure.Services.AuthServices
{
    public class AuthService(IAppDbContext context) : IAuthService 
    {
        private readonly IAppDbContext _context = context;

        public Task<string?> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
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
    }
}
