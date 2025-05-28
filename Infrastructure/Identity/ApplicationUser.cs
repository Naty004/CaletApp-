using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Domain;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        // Relación con Categorias e Ingresos
        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

          public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    }
}
