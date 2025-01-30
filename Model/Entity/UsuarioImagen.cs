using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public class UsuarioImagen
    {
        public string FotoPerfilUrl { get; set; }

        public UsuarioImagen() { }
        public UsuarioImagen(string url)
        {
            FotoPerfilUrl = url;
        }
    }
}
