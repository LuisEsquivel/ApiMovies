using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Dto.Usuario;
using ApiMovies.Helpers;
using ApiMovies.Interface.IGenericRepository;
using ApiMovies.Models;
using ApiMovies.Repository;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ApiMovies.Controllers
{
    [Route("api/usuario/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiUsuarios")]
    public class UsuariosController : Controller
    {

        private IGenericRepository<Usuario> repository;
        private IMapper mapper;
        private IConfiguration  config;
        private Response response;

        public UsuariosController(IMapper _mapper, ApplicationDbContext context, IConfiguration _congig)
        {
            this.mapper = _mapper;
            this.repository = new GenericRepository<Usuario>(context);
            this.config = _congig;
            this.response = new Response();
        }



        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns>StatusCode 200</returns>
        [HttpGet("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var dto = repository.GetAll();
            var user = new List<UsuarioDto>();

            foreach (var row in dto)
            {
                user.Add(mapper.Map<UsuarioDto>(row));
            }
            

          return Ok( user );
        }




        /// <summary>
        /// Obtener usuario por Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById(int Id)
        {
                return Ok( mapper.Map<UsuarioDto> ( repository.GetById(Id) ) );
        }




        /// <summary>
        /// Registro de usuario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] UsuarioAuthDto dto)
        {

            if(dto == null)
            {
                return BadRequest(StatusCodes.Status406NotAcceptable);
            }

            byte[] passwordHash, passwordSalt;
            CrearPassword(dto.Password, out passwordHash, out passwordSalt);


            var user = new UsuarioDto();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = dto.Email;

            var u = mapper.Map<Usuario>(user);

            if ( !repository.Add(u) )
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null, $"Algo salió mal guardar el registro: {user.Email}"));
            }

            return Ok( this.response.ResponseValues(this.Response.StatusCode, mapper.Map<UsuarioDto>(this.repository.GetById(u.Id))) );
        }




        /// <summary>
        /// Login con Email y Password
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] UsuarioAuthDto dto)
        {
            if (dto == null)
            {
                return Unauthorized();
            }

            var user = repository.GetByValues(x => x.Email == dto.Email).FirstOrDefault();

            if (!ValidatePassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized();
            }

            var claims = new[]
            {

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email.ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials       
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            }); ;
        }


        private bool ValidatePassword( string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                

                for(int i = 0; i < hashComputado.Length; i++)
                {
                    if(hashComputado[i] != passwordHash[i]) { return false;  }
                }

            }

            return true;
        }




        private void CrearPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {

                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

        }


    }
}
