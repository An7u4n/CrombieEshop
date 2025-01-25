namespace Model.Entity
{
    public class ProductoImagen
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string UrlImagen { get; set; }
        public virtual Producto Producto { get; set; }
        public ProductoImagen() { }
        public ProductoImagen(Producto producto, string urlImagen) 
        {
            ProductoId = producto.Id;
            UrlImagen = urlImagen;
            Producto = producto;
        }
    }
}
