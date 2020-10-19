using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            this.mapper  = _mapper;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var list = repository.GetAll();

            var listDto = new List<CategoriaDTO>();

            foreach(var row in list)
            {
                listDto.Add(mapper.Map<CategoriaDTO>(row));
            }


            return Ok(listDto);
        }

    }
}
