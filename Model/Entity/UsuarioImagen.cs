using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public class UsuarioImagen
    {
        public string FotoPerfilKey { get; set; }

        public UsuarioImagen() { }
        public UsuarioImagen(string key)
        {
            FotoPerfilKey = key;
        }
    }
}
