using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public  class Categorias: BaseEntity
    {
        public Categorias(string nombreCategoria, string descripcionCategoria, double porcentajeCategoria) 
        { 
            Id=Guid.NewGuid();
            NombreCategoria = nombreCategoria;
            DescripcionCategoria = descripcionCategoria;
            PorcentajeCategoria = porcentajeCategoria;
        }

        public string NombreCategoria { get; set; }
        public string DescripcionCategoria { get; set; }
        public double PorcentajeCategoria { get; set; }
    }
}
