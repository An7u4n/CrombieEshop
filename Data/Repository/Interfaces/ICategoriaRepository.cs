using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;
using Model.Entity;

namespace Data.Repository.Interfaces
{
    public interface ICategoriaRepository
    {
        void ActualizarCategoria(Categoria categoria);
        void EliminarCategoria(int idCategoria);
        Categoria CrearCategoria(Categoria categoria);
        Categoria ObtenerCategoria(int idCategoria);
        Categoria ObtenerCategoriaConProductos(int idCategoria);
        ICollection<Categoria> ObtenerCategorias();
    }
}
