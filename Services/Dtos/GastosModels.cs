using System;

namespace Services.Dtos
{
    public class GastosModels
    {
        public Guid Id { get; set; }

        public Guid CategoriaId { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public string UsuarioId { get; set; } = string.Empty;
    }
}
