using ApiMovies.Models;
using ApiMovies.Models.DTOs;
using AutoMapper;

namespace ApiMovies.Maps
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper() 
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
