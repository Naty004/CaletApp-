using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class LoginModel()
    {
        [Required]
        [EmailAddress]
        public required string CorreoElectronico { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Contrasena { get; set; }
    }
}
