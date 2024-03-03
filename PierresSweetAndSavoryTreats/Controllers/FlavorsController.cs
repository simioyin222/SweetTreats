using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PierresSweetAndSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq; 

namespace PierresSweetAndSavoryTreats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly ApplicationDbContext _db;

    public FlavorsController(ApplicationDbContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Title = "List of Flavor Tags";
      return View(_db.Flavors.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.Title = "Add a Flavor";
      return View();
    }


[HttpPost]
public async Task<IActionResult> Create(Flavor flavor)
{
    if (ModelState.IsValid)
    {
        _db.Flavors.Add(flavor);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(flavor);
}

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      ViewBag.Title = "Flavor Tag Details";
      Flavor targetFlavor = _db.Flavors.Include(entry => entry.JoinEntities)
                                       .ThenInclude(join => join.Treat)
                                       .FirstOrDefault(entry => entry.FlavorId == id);
      return View(targetFlavor);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Edit Flavor Tag";
      Flavor flavorToEdit = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      return View(flavorToEdit);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavorToUpdate)
    {
      if(ModelState.IsValid)
      {
        _db.Flavors.Update(flavorToUpdate);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavorToUpdate.FlavorId });
      }
      else
      {
        ViewBag.Title = "Edit Flavor Tag";
        return View(flavorToUpdate);
      }
    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Delete Flavor";
      Flavor flavorToDelete = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      return View(flavorToDelete);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor flavorToDelete = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      _db.Flavors.Remove(flavorToDelete);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      ViewBag.Title = "Add a Treat to This Flavor Tag";
      Flavor targetFlavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);
      ViewBag.TreatsList = _db.Treats.ToList();
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(targetFlavor);
    }

    [HttpPost]
    public ActionResult AddTreat(Flavor targetFlavor, int treatId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.FlavorId == targetFlavor.FlavorId && join.TreatId == treatId);
      #nullable disable
      if(joinEntity == null && treatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = targetFlavor.FlavorId, TreatId = treatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = targetFlavor.FlavorId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.FlavorId });
    }
  }
}