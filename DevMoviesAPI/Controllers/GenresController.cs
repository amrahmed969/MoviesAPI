

using DevMoviesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevMoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices _genresServices;
        public GenresController( IGenresServices genresServices)
        {            
            _genresServices = genresServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresServices.Getall();
            return Ok(genres); 
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre {Name=dto.Name};
            await _genresServices.Add(genre);
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody]GenreDto dto )
        {
            var genre = await _genresServices.GetById(id);
            if (genre == null)           
                return NotFound($"No Genre was found with ID:{id}");

            genre.Name = dto.Name;

            _genresServices.Update(genre);  

            return Ok(genre);         
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(byte id)
        {

            var genre = await _genresServices.GetById(id);
            if (genre == null)
                return NotFound($"No Genre was found with ID:{id}");
           _genresServices.Delete(genre);
            return Ok(genre);
        }
    }
}