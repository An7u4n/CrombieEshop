using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entity;

namespace Model.DTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ProductoDTO>? Productos { get; set; } = new List<ProductoDTO>();

        public CategoriaDTO() { }

        public CategoriaDTO(Categoria categoria)
        {
            Id = categoria.Id;
            Nombre = categoria.Nombre;
            Productos = categoria.Productos.Select(p => new ProductoDTO(p)).ToList();
        }
    }
}
