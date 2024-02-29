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
  public class FlavorsController : Controller
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorsController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
      _db = db;
      _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
      List<Flavor> flavors = _db.Flavors.ToList();
      return View(flavors);
    }

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Create(Flavor flavor)
    {
      if(!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
    public IActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
                                    .Include(flavor => flavor.JoinEntities)
                                    .ThenInclude(join => join.Treat)
                                    .FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    public IActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public IActionResult Edit(Flavor flavor)
    {
      if(!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
        _db.Flavors.Update(flavor);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }
    }

    public IActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public IActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);

      List<Treat> treats = _db.Treats.ToList();
      ViewBag.IsNoTreats = (treats.Count == 0) ? true : false;
      ViewBag.TreatId = new SelectList(treats, "TreatId", "Type");

      return View(thisFlavor);
    }

    [HttpPost]
    public IActionResult AddTreat(Flavor flavor, int treatId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.TreatId == treatId && join.FlavorId == flavor.FlavorId));
      #nullable disable
      if(joinEntity == null && treatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = flavor.FlavorId, TreatId = treatId });
        _db.SaveChanges();
      }

      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }

    [HttpPost]
    public IActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntity);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntity.FlavorId });
    }
  }
}