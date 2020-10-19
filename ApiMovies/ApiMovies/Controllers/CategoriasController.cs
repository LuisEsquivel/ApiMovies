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
    [Route("api/categorias/")]
    [ApiController]
    public class CategoriasController : Controller
    {


        private IGenericRepository<Categoria> repository;
        private IMapper mapper;

        public CategoriasController(IMapper _mapper, ApplicationDbContext context)
        {
            this.repository = new GenericRepository<Categoria>(context);
            this.mapper = _mapper;
        }


        [HttpGet("Get")]
        public IActionResult Get()
        {
            var list = repository.GetAll();

            var listDto = new List<CategoriaDTO>();

            foreach (var row in list)
            {
                listDto.Add(mapper.Map<CategoriaDTO>(row));
            }


            return Ok(listDto);
        }



        [HttpGet("GetById/{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var row = repository.GetById(Id);

            var rowDto = new List<CategoriaDTO>();

            rowDto.Add(mapper.Map<CategoriaDTO>(row));
            
            return Ok(rowDto);
        }


        [HttpPost("Add")]
        public IActionResult Add([FromBody] CategoriaDTO model)
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
                return StatusCode(404, ModelState);
            }

            return Ok();
        }


        [HttpPut("Update")]
        public IActionResult Update([FromBody] CategoriaDTO model)
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

            if (!repository.Update(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizar el registro: {model.Nombre}");
                return StatusCode(404, ModelState);
            }

            return Ok();
        }




        [HttpDelete("Delete/{Id:int}")]
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
                return StatusCode(404, ModelState);
            }

            return Ok();
        }
    }
}
