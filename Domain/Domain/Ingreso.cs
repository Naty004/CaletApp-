using System;

namespace Domain
{
    public class Ingreso : BaseEntity
    {
        public Ingreso(decimal monto, DateTime fecha, string usuarioId, string? descripcion = null)
        {
            Id = Guid.NewGuid();
            Monto = monto;
            Fecha = fecha;
            UsuarioId = usuarioId;
            Descripcion = descripcion;
        }


        public string? Descripcion { get; set; }

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // Relación con el usuario (solo por ID)
        public string UsuarioId { get; set; }
    }
}
