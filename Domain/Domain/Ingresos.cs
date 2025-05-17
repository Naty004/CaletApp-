using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Domain
{
    public class Ingresos: BaseEntity
    {
        public Ingresos(string descripcionIngreso, float montoIngreso, string fechaIngreso)
        {
            Id = Guid.NewGuid();
            DescripcionIngreso = descripcionIngreso;
            MontoIngreso = montoIngreso;
            FechaIngreso = fechaIngreso;
        }

        public string DescripcionIngreso { get; set; }
        public float MontoIngreso { get; set; }
        public string FechaIngreso { get; set; }

    }
}
