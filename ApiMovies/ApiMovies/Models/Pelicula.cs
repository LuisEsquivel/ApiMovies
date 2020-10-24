using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Models
{
    public class Pelicula
    {

        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho}

        public TipoClasificacion Clasificacion { get; set; }

        [Required (ErrorMessage ="La Categoría es requerida")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
