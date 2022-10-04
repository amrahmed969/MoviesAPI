namespace DevMoviesAPI.Services
{
    public interface IGenresServices
    {
        Task<IEnumerable<Genre>> Getall();
        Task<Genre> GetById(Byte id);
        Task<Genre> Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        Task<bool> IsValidGenre (Byte id);
    }
}
