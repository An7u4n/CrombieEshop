using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
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

        [HttpPost]
        public ActionResult CrearUsuario(UsuarioDTO usuario)
        {
            try
            {
                var usuarioCreado = _usuarioService.CrearUsuario(usuario);
                return Created("", usuarioCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
        [HttpPost("{id}/wishlist/{idProducto}")]
        public ActionResult AgregarItemAWishList(int id,int idProducto)
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
    }
}
