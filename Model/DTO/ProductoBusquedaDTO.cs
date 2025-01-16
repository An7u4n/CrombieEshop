using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ProductoBusquedaDTO
    {
        public int TotalPages { get; set; }
        public List<ProductoDTO> Productos { get; set; }
        public int CurrentPage { get; set; }
    }
}
