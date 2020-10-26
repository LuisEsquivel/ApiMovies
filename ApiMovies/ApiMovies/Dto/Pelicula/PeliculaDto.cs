using ApiMovies.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ApiMovies.Models.Pelicula;

namespace ApiMovies.Dto.Pelicula
{
    public class PeliculaDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public TipoClasificacion Clasificacion { get; set; }

        public int CategoriaId { get; set; }

    }
}
