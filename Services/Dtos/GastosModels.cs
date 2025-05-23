using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class GastosModels
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CategoriaModels")]
        public int CategoriaId { get; set; }
        public CategoriaModels CategoriaModels { get; set; } = new CategoriaModels();

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoGasto { get; set; }

        [Required]
        public DateTime FechaGasto { get; set; }

        public string DescripcionGasto { get; set; } 
    }
}
