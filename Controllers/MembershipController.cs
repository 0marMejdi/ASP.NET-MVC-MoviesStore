using Hakuna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hakuna.Controllers
{
    
public class MembershipController : Controller
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
    private readonly AppDbContext db;

    public MembershipController(AppDbContext _context)
    {
        db = _context;
    }
    public IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Add(Membership membership)
    {
        if (membership.DurationInMonth <= 0
            || membership.DiscountRate < 0
            || membership.Title == null
            || membership.SignUpFee < 0)
        {
            Fail("Membership cannot be Added due to input input!");
        }
        try
        {
            db.Add(membership);
            db.SaveChanges();
        }
        catch (Exception)
        {
            Fail("Membership cannot be Added due to database problem!");
        }
        Success("Membership has been added successfully!");
        return Add();
    }
    public IActionResult List()
    {
        List<Membership> membershiptypes = db.Membershiptypes.ToList();
        
        return View(membershiptypes);
    }
    public IActionResult Details(Guid id)
    {
        var membership = db.Membershiptypes.Find(id);
        if (membership == null)
            return List();
        return View(membership);
    }

}
}
