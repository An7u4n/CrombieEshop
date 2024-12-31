using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Service.Interfaces;

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

        [HttpGet]
        public ActionResult<ICollection<ProductoDTO>> ObtenerProductos()
        {
            try
            {
                var productos = _productoService.ObtenerProductos();
                return Ok(productos);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("{idProducto}")]
        public ActionResult<ProductoDTO> ObtenerProducto(int idProducto)
        {
            try
            {
                var producto = _productoService.ObtenerProducto(idProducto);
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult GuardarProducto(ProductoDTO productoDTO)
        {
            try
            {
                _productoService.GuardarProducto(productoDTO);
                return Created();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult ActualizarProducto(ProductoDTO productoDTO)
        {
            try
            {
                _productoService.ActualizarProducto(productoDTO);
                return Ok("Producto actualizado correctamente");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{idProducto}")]
        public ActionResult EliminarProducto(int idProducto)
        {
            try
            {
                _productoService.EliminarProducto(idProducto);
                return Ok("Producto eliminado correctamente");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
