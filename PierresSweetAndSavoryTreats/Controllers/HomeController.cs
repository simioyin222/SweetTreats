using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PierresSweetAndSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PierresSweetAndSavoryTreats.Controllers
{
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.Title = "Pierre's Sweet and Savory Treats";
      Flavor[] flavorsArray = _db.Flavors.ToArray();
      Treat[] treatsArray = _db.Treats.ToArray();
      Dictionary<string,object[]> model = new Dictionary<string,object[]>();
      model.Add("flavors", flavorsArray);
      model.Add("treats", treatsArray);
      return View(model);
    }
  }
}