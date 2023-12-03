using Hakuna.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hakuna.Repositories;

public class MovieRepository 
{
    private readonly AppDbContext _dbcontext;

    public MovieRepository(AppDbContext dbcontext)
    {
        this._dbcontext = dbcontext;
    }

    public List<Movie> GetAllMovies()
    {
        return _dbcontext.Movies.ToList();
    }

    public List<Movie> GetMoviesOrderedByNameThenReleaseDate()
    {
        return _dbcontext.Movies
            .OrderBy(m => m.Name)
            .ThenBy(m=>m.ReleaseDate)
            .ToList();
    }
    
    public List<Movie> GetMoviesByGenreAndCustomer(Guid genreId, Guid customerId)
    {
        return _dbcontext.Movies
            .Where(
                movie => movie.GenreID == genreId && movie.Customers.Any(
                    customer => customer.Id == customerId
                )
            )
            .ToList();
    }

    public List<Movie> GetMoviesByGenreId(Guid genreId)
    {
        return _dbcontext.Movies
            .Where(m => m.GenreID == genreId)
            .ToList();
    }
}
