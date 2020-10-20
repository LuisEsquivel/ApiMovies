using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Dto.Usuario
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Email No Valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordHash es requerida")]
        public byte[] PasswordHash { get; set; }

        [Required(ErrorMessage = "PasswordSalt es requerida")]
        public byte[] PasswordSalt { get; set; }
    }
}
