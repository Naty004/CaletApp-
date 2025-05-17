using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public class RegistroModels
    {
        public List<RegistroModel> Usuarios { get; set; }

        public RegistroModels()
        {
            Usuarios = new List<RegistroModel>();
        }
    }

    public class RegistroModel
    {
        public Guid Id { get; set; } // Necesario para edición y eliminación

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; }
    }
}