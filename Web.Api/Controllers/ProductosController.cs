using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly IS3Service _s3Service;
        public ProductosController(IProductoService productoService, IS3Service s3Service)
        {
            _productoService = productoService;
            _s3Service = s3Service;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/image")]
        async public Task<ActionResult> SubirImagen(int id, IFormFile file)
        {
            try
            {
                var producto = await _productoService.SubirImagen(id, file.OpenReadStream(), file.FileName, file.ContentType);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}
