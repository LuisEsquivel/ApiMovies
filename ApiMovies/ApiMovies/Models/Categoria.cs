using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Models
{
    public class Categoria
    {

        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }  

        public DateTime FechaCrecion { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

    }
}
