using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces;
using Model.DTO;
using Model.Entity;
using Service.Interfaces;

namespace Service
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _categoriaRepository = repository;
        }
        public void ActualizarCategoria(CategoriaDTO categoriaDTO)
        {
            var categoria = new Categoria(categoriaDTO);
            _categoriaRepository.ActualizarCategoria(categoria);
        }

        public CategoriaDTO CrearCategoria(CategoriaDTO productoDTO)
        {
            var categoria = new Categoria(productoDTO);
            var categoriaCreada = _categoriaRepository.CrearCategoria(categoria);
            return new CategoriaDTO(categoriaCreada);
        }

        public void EliminarCategoria(int idCategoria)
        {
            _categoriaRepository.EliminarCategoria(idCategoria);
        }

        public CategoriaDTO ObtenerCategoria(int idCategoria)
        {
            var categoria = _categoriaRepository.ObtenerCategoria(idCategoria);
            return new CategoriaDTO(categoria);
        }

        public ICollection<ProductoDTO> ObtenerProductosDeCategoria(int idCategoria)
        {
            var categoria = _categoriaRepository.ObtenerCategoriaConProductos(idCategoria);
            return categoria.Productos.Select(p => new ProductoDTO(p)).ToList();
        }

        public ICollection<CategoriaDTO> ObtenerCategorias()
        {
            var categorias = _categoriaRepository.ObtenerCategorias();
            var categoriasDTO = new List<CategoriaDTO>();
            return categorias.Select(c => new CategoriaDTO(c)).ToList();
        }
    }
}
