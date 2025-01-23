using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entity;
using Service;
using Service.Interfaces;
using System.Security.AccessControl;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<ICollection<ProductoDTO>> ObtenerProductos()
        {
            try
            {
                var productos = _productoService.ObtenerProductos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [Authorize]
        [HttpGet("search")]
        public ActionResult<ProductoBusquedaDTO> BuscarProductos([FromQuery] ProductoParametrosBusquedaDTO parametros)
        {
            try
            {
                var productos = _productoService.BuscarProductos(parametros);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ProductoDTO> ObtenerProducto(int id)
        {
            try
            {
                var producto = _productoService.ObtenerProducto(id);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}/categoria/{idCategoria}")]
        public ActionResult<ProductoDTO> AddCategoria(int id, int idCategoria)
        {
            try
            {
                var producto = _productoService.AddCategoria(id, idCategoria);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public ActionResult CrearProducto(ProductoDTO productoDTO)
        {
            try
            {
                var productoCreado = _productoService.CrearProducto(productoDTO);
                return Created("", productoCreado);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public ActionResult ActualizarProducto(int id, ProductoDTO productoDTO)
        {
            try
            {
                if (id != productoDTO.Id)
                {
                    return BadRequest("User ID mismatch.");
                }
                _productoService.ActualizarProducto(productoDTO);
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public ActionResult EliminarProducto(int id)
        {
            try
            {
                _productoService.EliminarProducto(id);
                return Ok("Producto eliminado correctamente");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("{idProducto}/imagen")]
        public async Task<ActionResult> SubirImagenProducto(int idProducto, IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest("No se cargo imagen.");
            }

            try
            {
                var imagenUrl = await _productoService.SubirImagenAsync( imagen.OpenReadStream(), imagen.FileName, idProducto, imagen.ContentType);
                return Ok($"Imagen de producto '{idProducto}' subida con exito. Encontrada en {imagenUrl}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
