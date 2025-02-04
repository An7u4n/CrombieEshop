using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.DTO.CarritoItemDTOs;
using Model.Entity;
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
                await _usuarioService.EliminarUsuario(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}/wishlist")]
        async public Task<ActionResult<ICollection<ProductoDTO>>> ListarItemsWishList(int id)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                var productos = _usuarioService.ListarItemsWishList(usuario.Id);
                return Ok(productos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("{id}/wishlist/{idProducto}")]
        async public Task<ActionResult> AgregarItemAWishList(int id, int idProducto)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                _usuarioService.AgregarItemsWishList(usuario.Id, idProducto);
                return Created();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}/wishlist/{idProducto}")]
        async public Task<ActionResult> EliminarItemWishList(int id, int idProducto)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                _usuarioService.EliminarItemsWishList(usuario.Id, idProducto);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}/carrito")]
        async public Task<ActionResult<List<CarritoItemDTO>>> ObtenerCarrito(int id)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                var carrito = _usuarioService.ObtenerCarrito(usuario.Id);
                return Ok(carrito);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}/carrito/{idProducto}")]
        async public Task<ActionResult> AgregarItemCarrito(int id, int idProducto, int Cantidad)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                var itemDTO = new SetCarritoItemDTO { UsuarioId = usuario.Id, ProductoId = idProducto, Cantidad = Cantidad };
                _usuarioService.AgregarItemCarrito(itemDTO);
                return Created();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}/carrito")]
        async public Task<ActionResult> AgregarItemsCarrito(int id, List<SetCarritoItemDTO> items)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                items.ForEach(i => i.UsuarioId = usuario.Id);
                _usuarioService.AgregarItemsCarrito(items);
                return Created();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}/carrito/{idProducto}")]
        async public Task<ActionResult> EliminarItemCarrito(int id, int idProducto)
        {
            try
            {
                var (accessToken, usuario) = await AuthorizeUser(id);
                _usuarioService.EliminarItemCarrito(usuario.Id, idProducto);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
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
                var (accessToken, usuario) = await AuthorizeUser(id);
                var url = await _usuarioService.SubirImagenPerfilAsync(imagen.OpenReadStream(), imagen.Name, imagen.ContentType, usuario.Id);
                var imgKey = _usuarioService.ObtenerFotoPerfilKey(usuario.Id);
                bool img = await _authService.ActualizarImagenPerfilKey(accessToken, imgKey);
                return Ok($"Foto de perfil subida en {url}");
            }
            catch (AmazonS3Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        private async Task<(string accessToken, UsuarioDTO userDTO)> AuthorizeUser(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userInfo = await _authService.ObtenerUsuarioDesdeAccessToken(accessToken);
            var userDTO = _usuarioService.ObtenerUsuarioPorEmail(userInfo.Attributes["email"]);
            if (userDTO.Id != id)
            {
                throw new UnauthorizedAccessException("No tienes permiso para realizar esta acción.");
            }
            return (accessToken, userDTO);
        }

    }
}
