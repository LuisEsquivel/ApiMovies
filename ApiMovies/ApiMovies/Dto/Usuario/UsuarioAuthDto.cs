using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Dto.Usuario
{
    public class UsuarioAuthDto
    {

        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Email No Valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password es requerida")]
        public string Password { get; set; }

    }
}
