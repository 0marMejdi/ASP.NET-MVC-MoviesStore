using Hakuna.Models;

namespace Hakuna.Services.ServicesContracts;

public interface IMovieService
{
    public List<Movie> GetMoviesByGenreAndCustomer(Guid genreId, Guid customerId);
    public List<Movie> GetAllMovies();
    public List<Movie> GetAllMoviesOrdered();
    public List<Movie> GetMovieByGenreId(Guid genreId);
    
}