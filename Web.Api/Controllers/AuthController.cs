﻿using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioService userService, IAuthService authService, ITokenService tokenService)
        {
            _usuarioService = userService;
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public ActionResult Register(UsuarioDTO dto)
        {
            try
            {
                var user = _authService.RegistrarUsuario(dto);
                var token = _tokenService.CreateJWTAuthToken(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("login")]
        public ActionResult Login(AuthDTO authData)
        {
            //Should create a token and return it
            try
            {
                var user = _authService.LoginUsuario(authData);
                if (user == null)
                {
                    return BadRequest("Invalid user or password");
                }
                var token = _tokenService.CreateJWTAuthToken(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
