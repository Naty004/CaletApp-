using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class CategoryModels
    {
        public List<CategoryModel> Categorias { get; set; }

        public CategoryModels()
        {
            Categorias = new List<CategoryModel>();
        }
    }

    public class CategoryModel()
    {

        [Required]
     
        public required string Nombre {  get; set; }

        [Required]
        [Description]
        public string Descripcion { get; set; }


    }

}
