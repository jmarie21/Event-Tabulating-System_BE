using ETS.Application.User.Commands;
using ETS.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_Tabulating_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(CreateUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _mediator.Send(command);
            if (user is null)
            {
                var errorResponse = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    {"Email", new[] {"Email already exists"}}
                });

                return BadRequest(errorResponse);
            }
               

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var token = await _mediator.Send(command);
            if (token is null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(token);  
        }
    }
}
