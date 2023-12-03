using Hakuna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Hakuna.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _dbcontext;
        public CustomerController(AppDbContext injectedContext)
        {
            _dbcontext = injectedContext;
        }
        public void  Success(string message)
        {
            ViewData["success"]=true;
            ViewData["message"] = message;
        }

        public void Fail(string message)
        {
            ViewData["fail"] = true;
            ViewData["message"] = message;
        }

        public IActionResult Add()
        {
            ICollection<Membership> memberships = _dbcontext.Membershiptypes.ToList();
            ViewBag.Members = new SelectList(memberships, "Id", "Title");
            return View(new Customer());
        }
        static bool IsValid(Customer customer)
        {
            if (customer.Name == null)
            {return false;}
            if (customer.LastName == null)
            {return false;}
            if (customer.Age < 16)
            {return false;}
            if (customer.MembershipId == null)
            {return false;}
            return true;
        }
        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (!IsValid(customer)){
                Fail("Can't Add Customer, check your input");
            }else{
                _dbcontext.Add(customer);
                _dbcontext.SaveChanges();
                Success("Added Customer successfully!");
            }
            return Add();
        }
        public IActionResult ListAll()
        {
            
            var customers = _dbcontext.Customers.ToList();
            
            return View("ListAll",customers);
        }

        public IActionResult Index()
        {
            return ListAll();
        }
        public IActionResult Details(Guid id)
        {
            var customer = _dbcontext.Customers
                .Include(c => c.Membershiptype)
                .FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return View(nameof(ListAll));
            }
            return View(customer);
        }

        public IActionResult Delete(Guid id)
        {
            var customer = _dbcontext.Customers.Find(id);
            if (customer == null)
            {
                Fail("Couldn't Delete Customer. Could be Already Deleted!");
            }
            else
            {
                _dbcontext.Customers.Remove(customer);
                _dbcontext.SaveChanges();
                Success("Customer Deleted Successfully");
            }
            
            return ListAll();
        }

        public IActionResult Edit(Guid id)
        {
            ICollection<Membership> memberships = _dbcontext.Membershiptypes.ToList();
            ViewBag.Members = new SelectList(memberships, "Id", "Title");
            var customer = _dbcontext.Customers.FirstOrDefault(c => c.Id==id);
            if (customer == null)
                return ListAll();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!IsValid(customer))
            {
                Fail("Cannot update customer due to input");
                return Edit(customer.Id);
            }
            Customer? existingCustomer = _dbcontext.Customers.Find(customer.Id);
            
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Update the properties of the existing movie with the values from the form
            _dbcontext.Entry(existingCustomer).CurrentValues.SetValues(customer);

            // Save changes to the database
            _dbcontext.SaveChanges();
            Success("Customer updated Successfully");
            return ListAll();
        }
        [Route("Customer/BuyMovie/{customerId}")]
        public IActionResult BuyMovie(Guid customerId)
        {
            var movies = _dbcontext.Movies.ToList();
            ViewBag.CustomerId = customerId;
            return View(movies);
        }
        [Route("Customer/BuyMovie/{customerId}/{movieId}")]
        public IActionResult BuyMovie(Guid customerId, Guid movieId)
        {
            var customer = _dbcontext.Customers
                .Include(customer1 => customer1.Movies)
                .FirstOrDefault(custo => custo.Id == customerId);
            var movie = _dbcontext.Movies.Find(movieId);
            if (customer == null)
            {
                Fail("Customer not found");
                return ListAll();
            }

            if (movie == null)
            {
                Fail("Movie not found");
                return ListAll();
            }
            if (!customer.Movies.Contains(movie))
            {
                customer.Movies.Add(movie);
                _dbcontext.SaveChanges();
                Success("Associated movie with client successfully!");
            }
            else
            {
                Fail("Customer already bought this movie");
            }

            return SeeMovies(customerId);
        }

        public IActionResult SeeMovies(Guid customerId)
        {
            var customer = _dbcontext.Customers
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                return ListAll();
            return View("SeeMovies",customer);
        }
    }
    
}