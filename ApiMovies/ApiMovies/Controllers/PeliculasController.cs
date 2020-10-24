using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Dto;
using ApiMovies.Helpers;
using ApiMovies.Interface.IGenericRepository;
using ApiMovies.Models;
using ApiMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting.Internal;

namespace ApiMovies.Controllers
{

    /// <summary>
    /// Peliculas Controller
    /// </summary>
    [Route("api/peliculas/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiMovies")]
    public class PeliculasController : ControllerBase
    {


        private IMapper mapper;
        private IGenericRepository<Pelicula> repository;
        private IWebHostEnvironment hostEnvironment;
        private Response response;


        /// <summary>
        /// Controlador de película
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="context"></param>
        /// <param name="_hostEnvironment"></param>
        public PeliculasController(IMapper _mapper, ApplicationDbContext context, IWebHostEnvironment _hostEnvironment)
        {
            repository = new GenericRepository<Pelicula>(context);
            mapper = _mapper;
            hostEnvironment = _hostEnvironment;
            this.response = new Response();
        }

        

        /// <summary>
        /// Obtener todas las películas
        /// </summary>
        /// <returns></returns>
        [HttpGet ("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
               var list = repository.GetAll();

                var listDto = new List<PeliculaAddDto>();

                foreach (var row in list)
                {
                    listDto.Add(mapper.Map<PeliculaAddDto>(row));
                }


                return Ok(this.response.ResponseValues(this.Response.StatusCode, listDto));
          
        }



        /// <summary>
        /// Obtener la película por Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Una película</returns>

        [HttpGet("GetById/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById(int Id)
        {
            return Ok( this.response.ResponseValues(this.Response.StatusCode , mapper.Map<PeliculaAddDto>(repository.GetById(Id))) );
        }




        /// <summary>
        /// Agregar una nueva película
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Status 200</returns>
        [HttpPost ("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromForm] PeliculaAddDto dto)
        {

            if(dto == null)
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable));
            }

            /*subir imágen*/
            var image = dto.ImageFile;
            var PathRoot = hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if(image.Length > 0)
            {
                var upload = Path.Combine(PathRoot, @"fotos");
                var extension = Path.GetExtension(files[0].FileName);
                var name = Guid.NewGuid().ToString();

                using (var FileStream = new FileStream(Path.Combine(upload, name + extension) , FileMode.Create ))
                {
                    files[0].CopyTo(FileStream);
                }

                dto.RutaImagen = @"fotos/" + name + extension;

            }
          

            if(repository.Exist(x => x.Nombre == dto.Nombre))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, $"Ya existe una pelicula con el Nombre: {dto.Nombre}"));
            }

            var row = mapper.Map<Pelicula>(dto);


            if (!repository.Add(row))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null, $"Algo salió mal al guardar la película: {dto.Nombre}"));
            }

            return Ok();

        }



        /// <summary>
        /// Actualizar la película
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Status 200</returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] PeliculaUpdateDto dto)
        {

            if (dto == null)
            {
                return BadRequest(StatusCodes.Status406NotAcceptable);
            }

            if (repository.Exist(x => x.Nombre == dto.Nombre && x.Id != dto.Id))
            {
               return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, $"Ya existe una película con el Nombre: {dto.Nombre}"));
            }

            /*subir imágen*/
            var image = dto.ImageFile;
            var PathRoot = hostEnvironment.WebRootPath;

       
            if (image != null)
            {

                var old = from o in repository.GetByValues(x => x.Id == dto.Id)
                          select o.RutaImagen;

                if(old != null)
                {
                    var OldPathComplete = Path.Combine(PathRoot + old);

                    if (System.IO.File.Exists(OldPathComplete))
                    {
                        System.IO.File.Delete(OldPathComplete);
                    }
                }

                var files = HttpContext.Request.Form.Files;
                var upload = Path.Combine(PathRoot, @"fotos");
                var extension = Path.GetExtension(files[0].FileName);
                var name = Guid.NewGuid().ToString();

                using (var FileStream = new FileStream(Path.Combine(upload, name + extension), FileMode.Create))
                {
                    files[0].CopyTo(FileStream);
                }

                dto.RutaImagen = @"fotos/" + name + extension;

            }


            var row = mapper.Map<Pelicula>(dto);

            if (!repository.Update(row, row.Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null, $"Algo salió mal al actualizar la película: {dto.Nombre}"));
            }

            return Ok( this.response.ResponseValues(this.Response.StatusCode, mapper.Map<PeliculaAddDto>(repository.GetById(row.Id))));

        }


        /// <summary>
        /// Acción de eliminar el campo Id es requerido
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Status Code 200</returns>
        [HttpDelete("Delete/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int Id)
        {

            if (Id == 0)
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, $"El parámetro Id es requerido"));
            }

            if(repository.Exist(x => x.Id == Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status404NotFound, null, "No existe la película"));
            }

            if (!repository.Delete(Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status404NotFound, null, "Algo salió mal al eliminar la película"));
            }

            return Ok( this.response.ResponseValues(this.Response.StatusCode)  );

        }




    }
}
