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
  public class TreatsController : Controller
  {
    private readonly ApplicationDbContext _db;

    public TreatsController(ApplicationDbContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      ViewBag.Title = "List of Treats";
      return View(_db.Treats.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.Title = "Add a Treat";
      return View();
    }

    [HttpPost]
public async Task<ActionResult> Create(Treat newlyAdded)
{
  if (ModelState.IsValid)
  {
    _db.Treats.Add(newlyAdded);
    try
    {
      await _db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
      // Log the exception (ex) here
      ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
    }
  }
  
  ViewBag.Title = "Add a Treat";
  return View(newlyAdded);
}

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      ViewBag.Title = "Treat Details";
      Treat targetTreat = _db.Treats.Include(entry => entry.JoinEntities)
                                    .ThenInclude(join => join.Flavor)
                                    .FirstOrDefault(entry => entry.TreatId == id);
      return View(targetTreat);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Edit Treat Details";
      Treat treatToEdit = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
      return View(treatToEdit);
    }

    [HttpPost]
    public ActionResult Edit(Treat targetTreatToEdit)
    {
      if(ModelState.IsValid)
      {
        _db.Treats.Update(targetTreatToEdit);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = targetTreatToEdit.TreatId });
      }
      else
      {
        ViewBag.Title = "Edit Treat Details";
        return View(targetTreatToEdit);        
      }

    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Delete Treat";
      Treat targetTreat = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
      return View(targetTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat treatToDelete = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
      _db.Treats.Remove(treatToDelete);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavorTag(int id)
    {
      ViewBag.Title = "Add a Flavor Tag to This Treat";
      Treat treatToAddTagsTo = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
      ViewBag.FlavorsList = _db.Flavors.ToList();
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(treatToAddTagsTo);
    }

    [HttpPost]
    public ActionResult AddFlavorTag(Treat entry, int flavorId)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.TreatId == entry.TreatId && join.FlavorId == flavorId);
      #nullable disable
      if (joinEntity == null && flavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = entry.TreatId, FlavorId = flavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = entry.TreatId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.TreatId });
    }
  }
}