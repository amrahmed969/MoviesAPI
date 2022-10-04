using AutoMapper;
using DevMoviesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase

    {
        private readonly IMoviesServices _MoviesServices;
        private readonly IGenresServices _GenresServices;
        private readonly IMapper _mapper;

        public MoviesController(IMoviesServices moviesServices, IGenresServices genresServices, IMapper mapper)
        {
            _MoviesServices = moviesServices;
            _GenresServices = genresServices;
            _mapper = mapper;
        }

        private new List<string> _allawedExtensions = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedPosterSize = 1048576;
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if (dto.Poster==null)
            {
                return BadRequest("poster is required");
            }
            if (!_allawedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jbg images are allowed" );

            if (dto.Poster.Length > _MaxAllowedPosterSize)
                return BadRequest("max allowed poster size 1MB");

            var isvalidgenre = await _GenresServices.IsValidGenre(dto.GenreId);
            if(!isvalidgenre)
                return BadRequest("Invalid genreID");

            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);
            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = datastream.ToArray();
            _MoviesServices.Add(movie); 
            return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsynv()
        {
            var movie = await _MoviesServices.GetAll();
            var data = _mapper.Map < IEnumerable < MovieDetailesDto>>(movie);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _MoviesServices.GetById(id);
            if (movie == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailesDto>(movie); 
            return Ok(dto);   
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreId(byte genreid)
        {
            var movie = await _MoviesServices.GetAll(genreid);
            var data = _mapper.Map<IEnumerable<MovieDetailesDto>>(movie);
            return Ok(data);
     
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _MoviesServices.GetById(id);
            var isvalidgenre = await _GenresServices.IsValidGenre(dto.GenreId); 
            if (!isvalidgenre)
                return BadRequest("Invalid genreID");

            if (movie == null)
                return NotFound($"no movie was found with id :{id}");
           
            if(dto.Poster != null)
            {
                if (!_allawedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jbg images are allowed");

                if (dto.Poster.Length > _MaxAllowedPosterSize)
                    return BadRequest("max allowed poster size 1MB");

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();
            }
          
            movie.Title=dto.Title;
            movie.Year=dto.Year;
            movie.GenreId=dto.GenreId;
            movie.StoryLine=dto.StoryLine;
            movie.Rate=dto.Rate;

            _MoviesServices.Update(movie);

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemovieAsync(int id)
        {
            var movie = await _MoviesServices.GetById(id);
            if (movie == null)
                return NotFound($"no movie was found with id :{id}");
            _MoviesServices.Delete(movie);
            return Ok(movie);
        }
    }
}
