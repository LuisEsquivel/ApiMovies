using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Dto;
using ApiMovies.Interface.IGenericRepository;
using ApiMovies.Mapper;
using ApiMovies.Models;
using ApiMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="context"></param>
        public CategoriasController(IMapper _mapper, ApplicationDbContext context)
        {
            this.repository = new GenericRepository<Categoria>(context);
            this.mapper = _mapper;
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
            var row = repository.GetById(Id);

            var rowDto = new List<CategoriaAddDto>();

            rowDto.Add(mapper.Map<CategoriaAddDto>(row));
            
            return Ok(rowDto);
        }



        /// <summary>
        /// Agregar una nueva categoría
        /// </summary>
        /// <param name="model"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] CategoriaAddDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (repository.GetAll().Where(x => x.Nombre == model.Nombre).ToList().Count > 0)
            {
                ModelState.AddModelError("", "La Categoría ya existe!!");
                return StatusCode(404, ModelState);
            }

            var categoria = mapper.Map<Categoria>(model);

            if (!repository.Add(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardar el registro: {model.Nombre}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }



        /// <summary>
        /// Actualizar categoría
        /// </summary>
        /// <param name="model"></param>
        /// <returns>StatusCode 200</returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] CategoriaUpdateDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (repository.GetAll().Where(x => x.Nombre == model.Nombre && x.Id != model.Id).ToList().Count > 0)
            {
                ModelState.AddModelError("", "La Categoría ya existe!!");
                return StatusCode(404, ModelState);
            }

            var categoria = mapper.Map<Categoria>(model);

            if (!repository.Update(categoria, categoria.Id))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizar el registro: {model.Nombre}");
                return StatusCode(500, ModelState);
            }

            return Ok();
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

                ModelState.AddModelError("", $"El parámetro (Id) es obligatorio");
                return StatusCode(404, ModelState);
                
            }
   

            if (repository.GetAll().Where(x => x.Id == Id).ToList().Count == 0)
            {
                ModelState.AddModelError("", $"La categoría con Id: {Id} No existe");
                return StatusCode(404, ModelState);
            }

            var delete =repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();

            var categoria = mapper.Map<Categoria>(delete);

            if (!repository.Delete(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal al eliminar el registro: {delete.Nombre}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
