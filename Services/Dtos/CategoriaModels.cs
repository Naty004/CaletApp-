using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class CategoriaModels
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string NombreCategoria { get; set; }
        
        [StringLength(250)]
        public string DescripcionCategoria { get; set; }
        [Required]
        public int PorcentajeCategoria { get; set; }

        //Relación con los gastos
        public List<GastosModels> Gastos { get; set; } = new List<GastosModels>();
    }
}
