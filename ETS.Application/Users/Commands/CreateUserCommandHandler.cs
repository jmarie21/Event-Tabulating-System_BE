using ETS.Application.Common.Interfaces;
using ETS.Application.User.Commands;
using ETS.Application.Users.DTOs;
using MediatR;


namespace ETS.Application.Users.Commands
{
    public class CreateUserCommandHandler(IAuthService authService) : IRequestHandler<CreateUserCommand, UserDto?>
    {
        private readonly IAuthService _authService = authService;
        public async Task<UserDto?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(request.FirstName!, request.LastName!, request.Email!, request.Password!);
        }
    }
}
