

using ETS.Application.Users.DTOs;

namespace ETS.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(string firstName, string lastName, string email, string password);
        Task<string?> LoginAsync(string email, string password);
    }
}
