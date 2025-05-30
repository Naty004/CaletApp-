using System;

namespace Domain
{
    public class Gasto : BaseEntity
    {
        public Gasto(string descripcion, decimal monto, DateTime fecha, Guid categoriaId, string usuarioId)
        {
            Id = Guid.NewGuid();
            Descripcion = descripcion;
            Monto = monto;
            Fecha = fecha;
            CategoriaId = categoriaId;
            UsuarioId = usuarioId;
        }

        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // Relación con Categoría
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Solo el ID del usuario (sin navegar a ApplicationUser)
        public string UsuarioId { get; set; }

        public Gasto() { }

    }

    
}
