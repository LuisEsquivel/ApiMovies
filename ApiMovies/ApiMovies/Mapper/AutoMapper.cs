using ApiMovies.Dto;
using ApiMovies.Dto.Categoria;
using ApiMovies.Dto.Pelicula;
using ApiMovies.Dto.Usuario;
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
            CreateMap<Usuario, UsuarioDto>().ReverseMap();

            CreateMap<Usuario, UsuarioAuthDto>().ReverseMap();



            CreateMap<Categoria, CategoriaDto>().ReverseMap();

            CreateMap<Categoria, CategoriaAddDto>().ReverseMap();

            CreateMap<Categoria, CategoriaUpdateDto>().ReverseMap();



            CreateMap<Pelicula, PeliculaDto>().ReverseMap();

            CreateMap<Pelicula, PeliculaAddDto>().ReverseMap();

            CreateMap<Pelicula, PeliculaUpdateDto>().ReverseMap();
        }

    }
}
