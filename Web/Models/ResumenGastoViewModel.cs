using System;

namespace Web.Models
{
    public class ResumenGastoViewModel
    {
        public Guid CategoriaId { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal GastoActual { get; set; }
        public decimal GastoMaximo { get; set; }

        public Guid GastoId { get; set; } //
        public string ColorSemaforo { get; set; } // green, yellow, red


        /// Calculado automáticamente para mostrar en la vista.
        /// Porcentaje redondeado a 2 decimales.
        public decimal PorcentajeUsado => GastoMaximo == 0 ? 0 : Math.Round((GastoActual / GastoMaximo) * 100, 2);

        /// Devuelve un color semáforo según el porcentaje usado.
        /// Verde: <70%, Amarillo: 70-90%, Rojo: >90%
       
    }
}
