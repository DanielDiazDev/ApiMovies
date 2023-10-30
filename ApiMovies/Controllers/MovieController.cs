using ApiMovies.Models;
using ApiMovies.Models.DTOs;
using ApiMovies.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var listMovies = _movieRepository.GetMovies();
            var listMoviesDto = new List<MovieDTO>();
            foreach (var item in listMovies)
            {
                listMoviesDto.Add(_mapper.Map<MovieDTO>(item));
            }
            return Ok(listMoviesDto);
        }
        [AllowAnonymous]
        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {
            var itemMovie = _movieRepository.GetMovie(movieId);

            if (itemMovie == null)
            {
                return NotFound();
            }

            var itemMovieDto = _mapper.Map<MovieDTO>(itemMovie);
            return Ok(itemMovieDto);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] MovieDTO moveieDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (moveieDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_movieRepository.HasMovie(moveieDTO.Name))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }
            var movie = _mapper.Map<Movie>(moveieDTO);
            if (!_movieRepository.AddMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{movie.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{movieId:int}", Name = "UpdatePatcMovie")]
        [ProducesResponseType(201, Type = typeof(MovieDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePatcMovie(int movieId, [FromBody] MovieDTO movieDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (movieDTO == null || movieId != movieDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDTO);

            if (!_movieRepository.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro{movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.HasMovie(movieId))
            {
                return NotFound();
            }

            var movie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro{movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("GetMoviesInCategory/{categoryId:int}")]
        public IActionResult GetMoviesInCategory(int categoryId)
        {
            var listaMovies = _movieRepository.GetMoviesInCategory(categoryId);

            if (listaMovies == null)
            {
                return NotFound();
            }

            var itemMovies = new List<MovieDTO>();

            foreach (var item in listaMovies)
            {
                itemMovies.Add(_mapper.Map<MovieDTO>(item));
            }
            return Ok(itemMovies);
        }
        [AllowAnonymous]
        [HttpGet("Find")]
        public IActionResult Find(string name)
        {
            try
            {
                var result = _movieRepository.FindMovies(name.Trim());
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
    }
}
