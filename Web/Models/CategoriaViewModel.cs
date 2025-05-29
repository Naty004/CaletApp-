using System;

namespace Web.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public double PorcentajeMaximoMensual { get; set; }
        public decimal GastoActual { get; set; }
        public decimal GastoMaximo { get; set; }
    }
}
