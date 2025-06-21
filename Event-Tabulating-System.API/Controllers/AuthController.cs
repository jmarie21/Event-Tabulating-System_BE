using Azure.Core;
using ETS.Application.User.Commands;
using ETS.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Event_Tabulating_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IConfiguration _configuration = configuration;
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
            var tokenDetails = await _mediator.Send(command);
            if (tokenDetails is null)
            {
                var errorResponse = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    {"Email", new[] {"Invalid Credentials"}}
                });

                return Unauthorized(errorResponse);
            }

            return Ok(new { token = tokenDetails });
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult ValidateToken(TokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // or true, based on setup
                ValidateAudience = false, // or true, based on setup
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(request.Token, validationParameters, out _);
                return Ok(new { isValid = true });
            }
            catch
            {
                return Ok(new { isValid = false });
            }
        }

        public class TokenRequest
        {
            public string? Token { get; set; }
        }
    }
}
