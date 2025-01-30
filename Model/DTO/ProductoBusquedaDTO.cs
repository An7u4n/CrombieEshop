using Model.Entity;

namespace Model.DTO
{


    public class ProductoBusquedaDTO
    {
        public int TotalPages { get; set; }
        public List<ProductoDTO> Productos { get; set; }
        public int CurrentPage { get; set; }
    }
}
