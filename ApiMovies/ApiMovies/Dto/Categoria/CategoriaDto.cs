using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Dto.Categoria
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCrecion { get; set; }
        public DateTime FechaActualizacion { get; set; } 
    }
}
