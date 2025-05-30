using System;

namespace Web.Models
{
    public class GastoViewModel
    {
        public Guid GastoId { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }

}