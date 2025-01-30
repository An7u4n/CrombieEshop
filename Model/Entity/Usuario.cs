using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Model.DTO;
using Model.Enums;

namespace Model.Entity
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Nombre { get; set; }
        [EmailAddress] public string Email { get; set; }
        public virtual List<WishListItem> WishListItems { get; set; } = [];

        public Usuario() { }

        public Usuario(UsuarioDTO dto)
        {
            Id = dto.Id;
            NombreDeUsuario = dto.NombreDeUsuario;
            Nombre = dto.Nombre;
            Email = dto.Email;
        }
    }
}
