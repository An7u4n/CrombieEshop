using Amazon.S3;
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
        private readonly IAuthService _authService;
        private readonly IS3Service _s3Service;
        public UsuarioController(IUsuarioService usuarioService, IAuthService authService, IS3Service s3Service)
        {
            _usuarioService = usuarioService;
            _authService = authService;
            _s3Service = s3Service;
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
        [HttpGet("{id}/foto-perfil")]
        public async Task<IActionResult> ObtenerFotoPerfil(int id)
        {
            try
            {
                var imgKey = _usuarioService.ObtenerFotoPerfilKey(id);
                var (stream, contentType) = await _s3Service.ObtenerArchivoAsync(imgKey);
                return File(stream, contentType);
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Image not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving image: {ex.Message}");
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        async public Task<ActionResult> EliminarUsuario(int id)
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
        [HttpPut("{id}/imagen")]
        public async Task<ActionResult> SubirImagenPerfil(int id, IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest("No se cargo imagen.");
            }

            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userInfo = await _authService.ObtenerUsuarioDesdeAccessToken(accessToken);
                var usuario = _usuarioService.ObtenerUsuarioPorEmail(userInfo.Attributes["email"]);
                if (usuario.Id != id)
                {
                    return Unauthorized("No tienes permiso para subir imagen a este usuario.");
                }
                var url = await _usuarioService.SubirImagenPerfilAsync(imagen.OpenReadStream(), imagen.Name, imagen.ContentType, usuario.Id);
                var imgKey = _usuarioService.ObtenerFotoPerfilKey(usuario.Id);
                bool img = await _authService.ActualizarImagenPerfilKey(accessToken, imgKey);
                return Ok($"Foto de perfil subida en {url}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
