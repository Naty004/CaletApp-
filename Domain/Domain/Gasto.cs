using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Gastos : BaseEntity
    {
        public Gastos(string descripcionGasto, float montoGasto, string fechaGasto)
        {
            Id = Guid.NewGuid();
            DescripcionGasto = descripcionGasto;
            MontoGasto = montoGasto;
            FechaGasto = fechaGasto;
        }

        public string DescripcionGasto { get; set; }
        public float MontoGasto { get; set; }
        public string FechaGasto { get; set; }

    }
}
