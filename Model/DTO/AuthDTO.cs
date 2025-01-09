using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AuthDTO
    {
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Contrasena { get; set; }
    }
}
