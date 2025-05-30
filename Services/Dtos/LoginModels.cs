using System.ComponentModel.DataAnnotations;

namespace Services.Dtos
{
    public class LoginModels
    {
        public List<LoginModel> Usuarios { get; set; }

        public LoginModels()
        {
            Usuarios = new List<LoginModel>();
        }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
