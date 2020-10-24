using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Dto;
using ApiMovies.Helpers;
using ApiMovies.Interface.IGenericRepository;
using ApiMovies.Mapper;
using ApiMovies.Models;
using ApiMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Validations;

namespace ApiMovies.Controllers
{


    /// <summary>
    /// Categorías Controller
    /// </summary>
    [Route("api/categorias/")]
    [ApiController]
    [ApiExplorerSettings (GroupName="ApiCategorias")]
    public class CategoriasController : Controller
    {


        private IGenericRepository<Categoria> repository;
        private IMapper mapper;
        private Response response;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="context"></param>
        public CategoriasController(IMapper _mapper, ApplicationDbContext context)
        {
            this.repository = new GenericRepository<Categoria>(context);
            this.mapper = _mapper;
            response = new Response();
        }




        /// <summary>
        ///  Obtener todas las categorías 
        /// </summary>
        /// <returns>StatusCode 200</returns>
        [HttpGet("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var list = repository.GetAll();

            var listDto = new List<CategoriaAddDto>();

            foreach (var row in list)
            {
                listDto.Add(mapper.Map<CategoriaAddDto>(row));
            }

            return Ok(listDto);
        }




        /// <summary>
        /// Obtener la categoría por el Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>StatusCode 200</returns>
        [HttpGet("GetById/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById(int Id)
        {     
            return Ok(mapper.Map<CategoriaAddDto>(repository.GetById(Id)));
        }



        /// <summary>
        /// Agregar una nueva categoría
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] CategoriaAddDto dto)
        {
            if (dto == null)
            {
                return BadRequest(StatusCodes.Status406NotAcceptable);
            }


            if (repository.Exist(x => x.Nombre == dto.Nombre))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, "La Categoría Ya Existe!!"));
            }

            var categoria = mapper.Map<Categoria>(dto);

            if (!repository.Add(categoria))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null , $"Algo salió mal guardar el registro: {dto.Nombre}"));
            }

            return Ok(
                         response.ResponseValues(this.Response.StatusCode,
                                                 mapper.Map<CategoriaAddDto>(repository.GetById(categoria.Id))
                                               )
                      );
        }



        /// <summary>
        /// Actualizar categoría
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] CategoriaUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(StatusCodes.Status406NotAcceptable);
            }

            if (repository.Exist(x => x.Nombre == dto.Nombre && x.Id != dto.Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, "La Categoría Ya Existe!!"));
            }

            var categoria = mapper.Map<Categoria>(dto);

            if (!repository.Update(categoria, categoria.Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null, $"Algo salió mal guardar el registro: {dto.Nombre}"));
            }




            return Ok(           
                       response.ResponseValues(this.Response.StatusCode,
                                               mapper.Map<CategoriaUpdateDto>(repository.GetById(categoria.Id))
                                             ) 
                    );

        }



        /// <summary>
        /// Eliminar una categoría por Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>StatusCode 200</returns>
        [HttpDelete("Delete/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, $"El parámetro (Id) es obligatorio"));
            }


            if (repository.Exist(x => x.Id == Id))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status406NotAcceptable, null, $"La categoría con Id: {Id} No existe"));
            }

            var row =repository.GetById(Id);

            var categoria = mapper.Map<Categoria>(row);

            if (!repository.Delete(categoria))
            {
                return BadRequest(this.response.ResponseValues(StatusCodes.Status500InternalServerError, null, $"Algo salió mal guardar el registro: {categoria.Nombre}"));

            }


            return Ok(  response.ResponseValues(this.Response.StatusCode) );
        }
    }
}
