using Hakuna.Models;
using Hakuna.Repositories;
using Hakuna.Services.ServicesContracts;

namespace Hakuna.Services.Services;

public class MovieService : IMovieService
{
    private readonly MovieRepository _repository;
    public MovieService(MovieRepository movieRepository)
    {
        _repository = movieRepository;
    }
    
    public List<Movie> GetMoviesByGenreAndCustomer(Guid genreId, Guid customerId)
    {
        return _repository.GetMoviesByGenreAndCustomer(genreId, customerId);
    }

    public List<Movie> GetAllMovies()
    {
        return _repository.GetAllMovies();
    }

    public List<Movie> GetAllMoviesOrdered()
    {
        return _repository.GetMoviesOrderedByNameThenReleaseDate();
    }

    public List<Movie> GetMovieByGenreId(Guid genreId)
    {
        return _repository.GetMoviesByGenreId(genreId);
    }
}
