using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Model.Entity;
using Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Model.DTO;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<ICollection<CategoriaDTO>> ObtenerCategorias()
        {
            try
            {
                var categorias = _categoriaService.ObtenerCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<CategoriaDTO> ObtenerCategoria(int id)
        {
            try
            {
                var categoria = _categoriaService.ObtenerCategoria(id);
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}/productos")]
        public ActionResult<ICollection<ProductoDTO>> ObtenerProductosDeCategoria(int id)
        {
            try
            {
                var productos = _categoriaService.ObtenerProductosDeCategoria(id);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CrearCategoria(CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoriaCreado = _categoriaService.CrearCategoria(categoriaDTO);
                return Created("", categoriaCreado);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult ActualizarCategoria(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                if (id != categoriaDTO.Id)
                {
                    return BadRequest("User ID mismatch.");
                }
                _categoriaService.ActualizarCategoria(categoriaDTO);
                return Ok(categoriaDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult EliminarCategoria(int id)
        {
            try
            {
                _categoriaService.EliminarCategoria(id);
                return Ok("Categoria eliminado correctamente");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
