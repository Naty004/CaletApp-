using System;

namespace Web.Models
{
    public class ResumenGastoViewModel
    {
        public Guid CategoriaId { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal GastoActual { get; set; }
        public decimal GastoMaximo { get; set; }

        public Guid GastoId { get; set; } // Opcional, si lo necesitas para enlaces

        /// <summary>
        /// Calculado automáticamente para mostrar en la vista.
        /// Porcentaje redondeado a 2 decimales.
        /// </summary>
        public decimal PorcentajeUsado => GastoMaximo == 0 ? 0 : Math.Round((GastoActual / GastoMaximo) * 100, 2);

        /// <summary>
        /// Devuelve un color semáforo según el porcentaje usado.
        /// Verde: <70%, Amarillo: 70-90%, Rojo: >90%
        /// </summary>
        public string ColorSemaforo
        {
            get
            {
                if (PorcentajeUsado >= 90) return "red";
                if (PorcentajeUsado >= 70) return "orange";
                return "green";
            }
        }
    }
}
