using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresSweetAndSavoryTreats.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace PierresSweetAndSavoryTreats.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FlavorsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Flavors.ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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

        public async Task<IActionResult> Details(int id)
        {
            var flavor = await _db.Flavors
                .FirstOrDefaultAsync(m => m.FlavorId == id);
            if (flavor == null)
            {
                return NotFound();
            }
            return View(flavor);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var flavor = await _db.Flavors.FindAsync(id);
            if (flavor == null)
            {
                return NotFound();
            }
            return View(flavor);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Flavor flavor)
        {
            if (id != flavor.FlavorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(flavor);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlavorExists(flavor.FlavorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flavor);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var flavor = await _db.Flavors
                .FirstOrDefaultAsync(m => m.FlavorId == id);
            if (flavor == null)
            {
                return NotFound();
            }

            return View(flavor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flavor = await _db.Flavors.FindAsync(id);
            _db.Flavors.Remove(flavor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlavorExists(int id)
        {
            return _db.Flavors.Any(e => e.FlavorId == id);
        }
    }
}