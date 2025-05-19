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
    public class SalongsController : Controller
    {
        private readonly CinemaContext _context;

        public SalongsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Salongs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Salonger.ToListAsync());
        }

        // GET: Salongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salong = await _context.Salonger
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salong == null)
            {
                return NotFound();
            }

            return View(salong);
        }

        // GET: Salongs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Seats")] Salong salong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salong);
        }

        // GET: Salongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salong = await _context.Salonger.FindAsync(id);
            if (salong == null)
            {
                return NotFound();
            }
            return View(salong);
        }

        // POST: Salongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Seats")] Salong salong)
        {
            if (id != salong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalongExists(salong.Id))
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
            return View(salong);
        }

        // GET: Salongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salong = await _context.Salonger
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salong == null)
            {
                return NotFound();
            }

            return View(salong);
        }

        // POST: Salongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salong = await _context.Salonger.FindAsync(id);
            if (salong != null)
            {
                _context.Salonger.Remove(salong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalongExists(int id)
        {
            return _context.Salonger.Any(e => e.Id == id);
        }
    }
}
