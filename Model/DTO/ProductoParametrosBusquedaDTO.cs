using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ProductoParametrosBusquedaDTO
    {
        public string? NombreProducto { get; set; }
        public double? PrecioMinimo { get; set; }
        public double? PrecioMaximo { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
