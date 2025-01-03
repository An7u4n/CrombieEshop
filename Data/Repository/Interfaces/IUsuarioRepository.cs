﻿using Model.Entity;

namespace Data.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        void ActualizarUsuario(Usuario usuario);
        void EliminarUsuario(int idUsuario);
        Usuario ObtenerUsuario(int idUsuario);
        void GuardarUsuario(Usuario usuario);
    }
}