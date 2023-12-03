
using Hakuna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using Hakuna.Services.ServicesContracts;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Hakuna.Controllers
{
    public class MovieController : Controller
    {
        public void  Success(string message)
        {
            ViewData["success"]=true;
            ViewData["message"] = message;
        }

        public void Fail(string message)
        {
            ViewData["fail"]=true;
            ViewData["message"] = message;
        }
        private readonly AppDbContext _context;
        private readonly IMovieService _movieService;

        public MovieController(AppDbContext _context, IMovieService movieService)
        {
            _movieService = movieService;
            this._context = _context;
        }
        public IActionResult Add()
        {

            ICollection<Genre> genres = _context.Genres.ToList();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");
            Movie movie = new Movie();
            return View(movie);
        }
        [HttpPost]
        public IActionResult Add( Movie movie)
        {
            Movie mov = new Movie();
            Console.Write(mov.Id.ToString()); 
            if (IsValid(movie))
            {
                if (movie.PosterPicture != null)
                {
                    var fileExtension = Path.GetExtension(movie.PosterPicture.FileName);
                    var filePath = Path.Combine("wwwroot", "uploads", movie.Name + ".jpg");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        movie.PosterPicture.CopyTo(stream);
                    }
                    
                }

                _context.Add(movie);
                _context.SaveChanges();
                Success("Movie Added Successfully!");
            }
            else
            {
                Fail("Can't add movie, check you input!");
            }
            return Add();
        }

        public IActionResult Members()
        {
            return RedirectToAction("Index","Membership");
        }
        public bool IsValid(Movie movie)
        {
            if (movie.Name == null)
                return false;
            if ( movie.ReleaseDate == null || movie.ReleaseDate <=0)
                return false;
            return true;
        }
        
        public IActionResult Index(){
            return RedirectToAction("ListAll");
        }
        public IActionResult ListAll(bool ordered = false, Guid genreId = new(), Guid customerId = new())
        {
            List<Movie> movies;
            if (ordered)
            {
                movies = _movieService.GetAllMoviesOrdered();
            }
            else
            {
                movies = _movieService.GetAllMovies();
            }

            if (genreId != new Guid())
            {
                if (customerId != new Guid())
                {
                    movies = _movieService.GetMoviesByGenreAndCustomer(genreId, customerId);
                }
                else
                {
                    movies = _movieService.GetMovieByGenreId(genreId);
                }
            }
            return View(nameof(ListAll),movies);
        }
        
        public IActionResult Details(Guid id)
        {   
            var movie = 
            _context.Movies
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.Id == id);
            if (movie==null)
            {
                return NotFound();
            }
            ViewBag.ImagePath = Path.Combine("wwwroot", "uploads", movie.Name + ".jpg");
            return View(movie);
                
        }
        public IActionResult Delete(Guid id){
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                Fail("Couldn't delete requested Movie, could be already deleted");

            }
            else
            {
                var existingPath = Path.Combine("wwwroot", "uploads", movie.Name + ".jpg");
                System.IO.File.Delete(existingPath);
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                Success("Movie Deleted Successfully");
            }

            return ListAll();

        }
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine("wwwroot", "uploads", imageName);
    
            if (! System.IO.File.Exists(imagePath))
            {
                imagePath = Path.Combine("wwwroot", "uploads", "_defaultMovie.jpg");
            }
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg"); // Adjust the content type based on your image type
        }

        public IActionResult Edit(Guid id)
        {
            ICollection<Genre> genres = _context.Genres.ToList();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");
            Movie? oldMovie = _context.Movies.Find(id);
            if (oldMovie == null)
                return ListAll();
            return View(oldMovie);
        }
        [HttpPost]
        public IActionResult Edit(Movie newMovie)
        {
            if (!IsValid(newMovie))
            {
                Fail("Cannot update customer due to input");
                return Edit(newMovie.Id);
            }
            Movie? existingMovie = _context.Movies.Find(newMovie.Id);

            if (existingMovie == null)
            {
                return NotFound();
            }
            if (newMovie.PosterPicture != null )
            {
                var existingPath = Path.Combine("wwwroot", "uploads", existingMovie.Name + ".jpg");
                System.IO.File.Delete(existingPath);
                Console.WriteLine(existingPath + " is " + (System.IO.File.Exists(existingPath)?"yes":"no"));
                var filePath = Path.Combine("wwwroot", "uploads", newMovie.Name + ".jpg");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newMovie.PosterPicture.CopyTo(stream);
                }
            }
            else
            {
                var existingPath = Path.Combine("wwwroot", "uploads", existingMovie.Name + ".jpg");
                var filePath = Path.Combine("wwwroot", "uploads", newMovie.Name + ".jpg");

                System.IO.File.Move(existingPath,filePath);
            }
            // Update the properties of the existing movie with the values from the form
            _context.Entry(existingMovie).CurrentValues.SetValues(newMovie);
            
            
            // Save changes to the database
            _context.SaveChanges();
            Success("Customer updated Successfully");
            return ListAll();
        }

    }
}
