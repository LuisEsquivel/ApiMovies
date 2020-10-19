using ApiMovies.Dto;
using ApiMovies.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovies.Mapper
{
    public class AutoMappers: Profile
    {

        public AutoMappers()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }

    }
}
