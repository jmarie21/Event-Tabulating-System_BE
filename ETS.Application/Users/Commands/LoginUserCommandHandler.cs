using ETS.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Application.Users.Commands
{
    public class LoginUserCommandHandler(IAuthService authService) : IRequestHandler<LoginUserCommand, string?>
    {
        private readonly IAuthService _authService = authService;
        public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.Email!, request.Password!);
        }
    }
}
