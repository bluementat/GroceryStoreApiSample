using GroceryStoreAPI.DataAccess;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IUserRepository repository;
        private readonly ITokenService tokenService;
        private string generatedToken = null;

        public AuthenticateController(IConfiguration Config, ITokenService TokenService, IUserRepository UserRepository)
        {
            config = Config;
            tokenService = TokenService;
            repository = UserRepository;
        }

        /// <summary>
        /// Login in to recieve authentication token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/authenticate/login
        ///     {
        ///         "Name": "[user name]",
        ///         "Password": "[password]"
        ///     }
        ///
        /// </remarks>        
        /// <returns>Returns authentication tokent to access API</returns>
        /// <response code="200">Returns authentication token</response>   
        /// <response code="403">Login failed - not authorized</response>           
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public ActionResult Login(ApiUser apiUser)
        {
            if (string.IsNullOrEmpty(apiUser.Name) || string.IsNullOrEmpty(apiUser.Password))
            {
                return BadRequest();
            }

            ActionResult response = Unauthorized();

            var validUser = repository.Getuser(apiUser);

            if (validUser.Name != string.Empty)
            {
                generatedToken = tokenService.BuildToken(config["Jwt:Key"].ToString(), config["Jwt:Issuer"].ToString(), validUser);
                if (generatedToken != null)
                {
                    return Ok(generatedToken);
                }
                else
                {
                    return StatusCode(503);
                }
            }
            else
            {
                return Forbid();
            }
        }
    }
}
