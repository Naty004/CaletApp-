using System;
using System.Collections.Generic;

namespace Domain
{
    public class Categoria : BaseEntity
    {
        // Constructor vacío requerido por EF Core
        public Categoria()
        {
            Visible = true;
        }

        // Constructor con parámetros para uso manual
        public Categoria(string nombreCategoria, string? descripcionCategoria, double porcentajeCategoria, string usuarioId)
        {
            Id = Guid.NewGuid();
            Nombre = nombreCategoria;
            Descripcion = descripcionCategoria;
            PorcentajeMaximoMensual = porcentajeCategoria;
            UsuarioId = usuarioId;
            Visible = true;
        }

        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public double PorcentajeMaximoMensual { get; set; }
        public bool Visible { get; set; }

        // Solo el ID del usuario
        public string UsuarioId { get; set; }

        // Relación con gastos
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
