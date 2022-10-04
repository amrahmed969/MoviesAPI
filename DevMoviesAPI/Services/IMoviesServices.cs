namespace DevMoviesAPI.Services
{
    public interface IMoviesServices
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie Movie);
        Movie Update(Movie Movie);
        Movie Delete(Movie movie);

    }
}
