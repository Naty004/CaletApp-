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
        public Guid Id { get; set; } // Se puede usar para edición o identificación

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [Display(Name = "Apellido")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmarContrasena { get; set; }
    }
}
