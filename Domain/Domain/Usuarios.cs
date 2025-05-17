using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Usuarios: BaseEntity
    {

        public Usuarios(string nombres, string apellidos, string nombreUsuario, string correoElectronico, string contrasena)
        {
            Id = Guid.NewGuid();
            Nombres = nombres;
            Apellidos = apellidos;
            NombreUsuario = nombreUsuario;
            CorreoElectronico = correoElectronico;
            Contrasena = contrasena;
        }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }

    }
}
