using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Dto
{
    public class CategoriaAddDto
    {

        public int Id { get; set; }

        [Required (ErrorMessage = "El Nombre es Requerido")]
        public string Nombre { get; set; }

        public DateTime FechaCreacion{ get; set; } = DateTime.Now;
    }
}
