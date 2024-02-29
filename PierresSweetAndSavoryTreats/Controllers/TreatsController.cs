using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresSweetAndSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresSweetAndSavoryTreats.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
      List<Treat> treats = _db.Treats.ToList();
      return View(treats);
    }

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Create(Treat treat)
    {
      if(!ModelState.IsValid)
      {
        return View(treat);
      }
      else
      {
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public IActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                                  .Include(treat => treat.JoinEntities)
                                  .ThenInclude(join => join.Flavor)
                                  .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    public IActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public IActionResult Edit(Treat treat)
    {
      if(!ModelState.IsValid)
      {
        return View(treat);
      }
      else
      {
        _db.Treats.Update(treat);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = treat.TreatId });
      }
    }

    public IActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed (int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public IActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);

      List<Flavor> flavors = _db.Flavors.ToList();
      ViewBag.IsNoFlavors = (flavors.Count == 0) ? true : false;
      ViewBag.FlavorId = new SelectList(flavors, "FlavorId", "Type");

      return View(thisTreat);
    }

    [HttpPost]
    public IActionResult AddFlavor(Treat treat, int flavorId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable

      if(joinEntity == null && flavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = flavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [HttpPost]
    public IActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntity);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntity.TreatId });
    }
  }
}