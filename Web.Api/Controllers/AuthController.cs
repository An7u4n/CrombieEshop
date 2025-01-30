using Microsoft.AspNetCore.Mvc;
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

        public AuthController(IUsuarioService userService, IAuthService authService)
        {
            _usuarioService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UsuarioDTO dto)
        {
            try
            {
                await _authService.RegistrarAsync(dto.Email, dto.Contrasena);
                UsuarioDTO user = _usuarioService.CrearUsuario(dto);
                return Created(user.Id.ToString(), user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("confirm-signup")]
        public async Task<ActionResult> ConfirmSignUp(ConfirmSignUpDTO dto)
        {
            try
            {
                await _authService.ConfirmarRegistroAsync(dto.Email, dto.Code);
                return Ok(dto.Email + " confirmado con exito");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("login")]
        public async  Task<ActionResult<string>> Login(AuthDTO authData)
        {
            try
            {
                if(authData.Email == null)
                {
                    return BadRequest("Invalid email or password");
                }
                var result = await _authService.IniciarSesion(authData.Email, authData.Contrasena);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
