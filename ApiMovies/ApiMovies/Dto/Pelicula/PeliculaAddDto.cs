using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ApiMovies.Models.Pelicula;

namespace ApiMovies.Dto
{
    public class PeliculaAddDto
    {

        public int Id { get; set; }

        [Required (ErrorMessage ="El nombre es requerido") ]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripción es requerida")]
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }

        public IFormFile ImageFile { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        public TipoClasificacion Clasificacion { get; set; } = 0;

        [Required(ErrorMessage = "La Categoría es requerida")]
        public int CategoriaId { get; set; }

    }
}
