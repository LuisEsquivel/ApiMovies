using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Dto
{
    public class CategoriaDTO
    {

        public int Id { get; set; }

        [Required (ErrorMessage = "El Nombre es Obligatorio")]
        public string Nombre { get; set; }

    }
}
