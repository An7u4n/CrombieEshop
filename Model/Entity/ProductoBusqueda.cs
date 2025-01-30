namespace Model.Entity
{
    public class ProductoBusqueda
    {
        public int TotalPages { get; set; }
        public List<Producto> Productos { get; set; }
        public int CurrentPage { get; set; }
    }
}
