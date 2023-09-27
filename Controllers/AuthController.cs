using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hustle.Interfaces;
using Hustle.Repositories;
using Hustle.Repositories.Auth;

namespace Hustle.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository repository)
        {
            _authRepository = repository;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserRegistration input) => Ok(_authRepository.Register(input));
        
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLogin input) => Ok(_authRepository.SignIn(input));

        [HttpGet("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] UserLogin input) => Ok(_authRepository.ChangePassword(input));

    }
}