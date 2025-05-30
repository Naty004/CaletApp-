using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class GastoCreateViewModel
    {
        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public Guid CategoriaId { get; set; }

        public List<CategoriaItem> Categorias { get; set; } = new List<CategoriaItem>();

        public class CategoriaItem
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; }
        }
    }
}
