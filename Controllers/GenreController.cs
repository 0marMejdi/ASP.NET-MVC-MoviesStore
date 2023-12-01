
using System.ComponentModel.DataAnnotations;
using Hakuna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
namespace Hakuna.Controllers
{
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;
        public GenreController(AppDbContext context)
        {
            this._context = context;
        }
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
        public IActionResult Add()
        {
            Genre genre = new Genre();
            return View("Add",genre);
        }
        [HttpPost]
        public IActionResult Add( Genre genre)
        {
            if (genre.Name != null)
            {
                _context.Add(genre);
                _context.SaveChanges();
                Success("Added Genre Successfully!");
            }
            else
            {
                Fail("Couldn't add Genre, please check your input!");
            }

            return Add();
        }
        public IActionResult Index(){
            return RedirectToAction("ListAll","Movie");
        }
         public IActionResult ListAll(){
            List<Genre> genres = _context.Genres.ToList();
            _context.SaveChanges();
            return View(genres);
        } 
        /* public IActionResult Details(Guid id){
           
            var movies = context.Movies.ToList();            
            foreach (var el in movies){
                if (el.Id==id){
                    return View(el);
                }
            }
            return NotFound();
            
        }*/ 
        /* public IActionResult Delete(Guid id){
            var movie = context.Movies.Find(id);
            if (movie is not null)
            {
                context.Movies.Remove(movie);
                context.SaveChanges();
            }
            return RedirectToAction("ListAll");
            
        } */ 
    }
}