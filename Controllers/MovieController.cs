
using Hakuna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
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
        public MovieController(AppDbContext context)
        {
            this._context = context;
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
            if (movie.ReleaseDate <=0)
                return false;
            return true;
        }
        
        
        public IActionResult Index(){
            return RedirectToAction("ListAll");
        }
        public IActionResult ListAll(){
            List<Movie> movies = _context.Movies.ToList();
            _context.SaveChanges();
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
    }
}
