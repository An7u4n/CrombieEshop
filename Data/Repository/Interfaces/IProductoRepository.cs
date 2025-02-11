﻿using Model.DTO;
using Model.Entity;

namespace Data.Repository.Interfaces
{
    public interface IProductoRepository
    {
        void ActualizarProducto(Producto producto);
        void EliminarProducto(int idProducto);
        Producto CrearProducto(Producto producto);
        Producto ObtenerProducto(int idProducto);
        ICollection<Producto> ObtenerProductos();

        ICollection<Producto> ObtenerProductos(ICollection<int> idsProductos);
        ProductoBusqueda BuscarProductos(ProductoParametrosBusquedaDTO parametros);
    }
}