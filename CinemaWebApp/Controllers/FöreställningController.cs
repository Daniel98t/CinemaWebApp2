using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Data;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class FöreställningController : Controller
    {
        private readonly CinemaContext _context;

        public FöreställningController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Föreställning
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Föreställningar.Include(f => f.Film).Include(f => f.Salong);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Föreställning/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var föreställning = await _context.Föreställningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (föreställning == null)
            {
                return NotFound();
            }

            return View(föreställning);
        }

        // GET: Föreställning/Create
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title");
            ViewData["SalongId"] = new SelectList(_context.Salonger, "Id", "Number");
            return View();
        }

        // POST: Föreställning/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FilmId,SalongId,Time")] Föreställning föreställning)
        {
            if (ModelState.IsValid)
            {
                _context.Add(föreställning);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", föreställning.FilmId);
            ViewData["SalongId"] = new SelectList(_context.Salonger, "Id", "Number", föreställning.SalongId);
            return View(föreställning);
        }

        // GET: Föreställning/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var föreställning = await _context.Föreställningar.FindAsync(id);
            if (föreställning == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", föreställning.FilmId);
            ViewData["SalongId"] = new SelectList(_context.Salonger, "Id", "Number", föreställning.SalongId);
            return View(föreställning);
        }

        // POST: Föreställning/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilmId,SalongId,Time")] Föreställning föreställning)
        {
            if (id != föreställning.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(föreställning);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FöreställningExists(föreställning.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", föreställning.FilmId);
            ViewData["SalongId"] = new SelectList(_context.Salonger, "Id", "Number", föreställning.SalongId);
            return View(föreställning);
        }

        // GET: Föreställning/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var föreställning = await _context.Föreställningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (föreställning == null)
            {
                return NotFound();
            }

            return View(föreställning);
        }

        // POST: Föreställning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var föreställning = await _context.Föreställningar.FindAsync(id);
            if (föreställning != null)
            {
                _context.Föreställningar.Remove(föreställning);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Föreställningar för en specifik film
        public async Task<IActionResult> VisaForestallningar(int filmId)
        {
            // Hämta föreställningar med tillhörande Film och Salong
            var föreställningar = await _context.Föreställningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .Where(f => f.FilmId == filmId)
                .ToListAsync();

            if (!föreställningar.Any())
            {
                return NotFound("Inga föreställningar hittades för denna film.");
            }

            // Beräkna tillgängliga platser för varje föreställning
            foreach (var föreställning in föreställningar)
            {
                föreställning.Salong.Seats -= _context.Bokningar.Count(b => b.FöreställningId == föreställning.Id);
            }

            // Skicka filmens titel till vyn via ViewBag
            ViewBag.FilmTitle = föreställningar.FirstOrDefault()?.Film?.Title ?? "Okänd Film";

            return View(föreställningar);
        }



        private bool FöreställningExists(int id)
        {
            return _context.Föreställningar.Any(e => e.Id == id);
        }
    }
}
