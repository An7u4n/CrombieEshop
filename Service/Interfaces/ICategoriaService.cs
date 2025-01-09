using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;

namespace Service.Interfaces
{
    public interface ICategoriaService
    {
        void ActualizarCategoria(CategoriaDTO categoriaDTO);
        void EliminarCategoria(int idCategoria);
        CategoriaDTO CrearCategoria(CategoriaDTO productoDTO);
        CategoriaDTO ObtenerCategoria(int idCategoria);
        CategoriaDTO ObtenerCategoriaConProductos(int idCategoria);
        ICollection<CategoriaDTO> ObtenerCategorias();
    }
}
