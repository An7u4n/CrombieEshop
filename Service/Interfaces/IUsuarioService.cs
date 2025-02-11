﻿using Model.DTO;
using Model.DTO.CarritoItemDTOs;

namespace Service.Interfaces
{
    public interface IUsuarioService
    {
        void ActualizarUsuario(UsuarioDTO usuarioDTO);
        Task EliminarUsuario(int idUsuario);
        UsuarioDTO CrearUsuario(UsuarioDTO usuarioDTO);
        UsuarioDTO ObtenerUsuario(int idUsuario);
        UsuarioDTO ObtenerUsuarioPorEmail(string email);
        string ObtenerFotoPerfilKey(int idUsuario);
        ICollection<UsuarioDTO> ObtenerUsuarios();
        void AgregarItemsWishList(int idUsuario, int idProducto);
        void EliminarItemsWishList(int idUsuario, int idProducto);
        ICollection<ProductoDTO> ListarItemsWishList(int idUsuario);
        Task<string> SubirImagenPerfilAsync(Stream fileStream, string fileName, string contentType, int userId);
        void AgregarItemCarrito(SetCarritoItemDTO itemDTO);
        void AgregarItemsCarrito(ICollection<SetCarritoItemDTO> itemsDTO);
        void EliminarItemCarrito(int idUsuario, int idProducto);
        List<CarritoItemDTO> ObtenerCarrito(int idUsuario);

    }
}