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

        [HttpPost]
        public ActionResult CrearUsuario(UsuarioDTO usuario)
        {
            try
            {
                _usuarioService.CrearUsuario(usuario);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult ActualizarUsuario(UsuarioDTO usuario)
        {
            try
            {
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
