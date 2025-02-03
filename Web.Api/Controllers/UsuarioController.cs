using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Service;
using Service.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public ActionResult<ICollection<UsuarioDTO>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = _usuarioService.ObtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public ActionResult ActualizarUsuario(int id, UsuarioDTO usuario)
        {
            try
            {
                if (id != usuario.Id)
                {
                    return BadRequest("User ID mismatch.");
                }

                _usuarioService.ActualizarUsuario(usuario);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("{id}")]
        public ActionResult<UsuarioDTO> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = _usuarioService.ObtenerUsuario(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public ActionResult EliminarUsuario(int id)
        {
            try
            {
                _usuarioService.EliminarUsuario(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}/wishlist")]
        public ActionResult<ICollection<ProductoDTO>> ListarItemsWishList(int id)
        {
            try
            {
                var productos = _usuarioService.ListarItemsWishList(id);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("{id}/wishlist/{idProducto}")]
        public ActionResult AgregarItemAWishList(int id, int idProducto)
        {
            try
            {
                _usuarioService.AgregarItemsWishList(id, idProducto);
                return Created();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}/wishlist/{idProducto}")]
        public ActionResult EliminarItemWishList(int id, int idProducto)
        {
            try
            {
                _usuarioService.EliminarItemsWishList(id, idProducto);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("/imagen")]
        public async Task<ActionResult> SubirImagenPerfil(IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest("No se cargo imagen.");
            }

            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var url = await _usuarioService.SubirImagenPerfilAsync(imagen.OpenReadStream(), imagen.Name, imagen.ContentType, accessToken);
                return Ok($"Foto de perfil subida en {url}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
