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
        public CategoriaDTO() { }

        public CategoriaDTO(Categoria categoria)
        {
            Id = categoria.Id;
            Nombre = categoria.Nombre;
        }
    }
}
