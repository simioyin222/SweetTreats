using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PierresSweetAndSavoryTreats.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace PierresSweetAndSavoryTreats.Controllers
{
    public class TreatsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TreatsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Treats
        public async Task<IActionResult> Index()
        {
            return View(await _db.Treats.ToListAsync());
        }

        // GET: Treats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TreatId,Name")] Treat treat)
        {
            if (ModelState.IsValid)
            {
                _db.Add(treat);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treat);
        }

        // GET: Treats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treat = await _db.Treats.FindAsync(id);
            if (treat == null)
            {
                return NotFound();
            }
            return View(treat);
        }

        // POST: Treats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatId,Name")] Treat treat)
        {
            if (id != treat.TreatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(treat);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatExists(treat.TreatId))
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
            return View(treat);
        }
    }
}