using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Dto;
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
    [Route("api/peliculas/")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {


        private IMapper mapper;
        private IGenericRepository<Pelicula> repository;
        private IWebHostEnvironment hostEnvironment;


        public PeliculasController(IMapper _mapper, ApplicationDbContext context, IWebHostEnvironment _hostEnvironment)
        {
            repository = new GenericRepository<Pelicula>(context);
            mapper = _mapper;
            hostEnvironment = _hostEnvironment;
        }

        
        [HttpGet ("Get")]
        public IActionResult Get()
        {
            var list = repository.GetAll();

            var listDto = new List<PeliculaDTO>();

            foreach (var row in list)
            {
                listDto.Add(mapper.Map<PeliculaDTO>(row));
            }


            return Ok(listDto);

        }



        [HttpGet("GetById/{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var row = repository.GetById(Id);

            var rowDto = new List<PeliculaDTO>();

            rowDto.Add(mapper.Map<PeliculaDTO>(row));

            return Ok(rowDto);
        }



        [HttpPost ("Add")]
        public IActionResult Add([FromForm] PeliculaDTO dto)
        {

            if(dto == null)
            {
                return BadRequest();
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
          

            if(repository.GetAll().Where(x => x.Nombre == dto.Nombre).ToList().Count > 0)
            {
                ModelState.AddModelError("", $"Ya existe una pelicula con el Nombre: {dto.Nombre}");
                return StatusCode(404, ModelState);
            }

            var row = mapper.Map<Pelicula>(dto);


            if (!repository.Add(row))
            {
                ModelState.AddModelError("", $"Algo salió mal al guardar la película: {dto.Nombre}");
                return StatusCode(404, ModelState);
            }

            return Ok();

        }


        [HttpPut("Update")]
        public IActionResult Update([FromBody] PeliculaDTO dto)
        {

            if (dto == null)
            {
                return BadRequest();
            }

            if (repository.GetAll().Where(x => x.Nombre == dto.Nombre && x.Id == dto.Id).ToList().Count > 0)
            {
                ModelState.AddModelError("", $"Ya existe una película con el Nombre: {dto.Nombre}");
                return StatusCode(404, ModelState);
            }

            var row = mapper.Map<Pelicula>(dto);


            if (!repository.Add(row))
            {
                ModelState.AddModelError("", $"Algo salió mal al actualizar la película: {dto.Nombre}");
                return StatusCode(404, ModelState);
            }

            return Ok();

        }

        [HttpDelete("Delete/{Id:int}")]
        public IActionResult Delete(int Id)
        {

            if (Id == 0)
            {
                ModelState.AddModelError("", $"El parámetro Id es requerido");
                return StatusCode(404, ModelState);
            }

            if(repository.GetAll().Where(x => x.Id == Id).ToList().Count == 0)
            {
                ModelState.AddModelError("", $"No existe la película");
                return StatusCode(404, ModelState);
            }

            if (!repository.Delete(Id))
            {
                ModelState.AddModelError("", $"Algo salió mal al eliminar la película");
                return StatusCode(404, ModelState);
            }

            return Ok();

        }




    }
}
