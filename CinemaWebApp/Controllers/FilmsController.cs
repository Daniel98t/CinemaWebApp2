using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Data;
using CinemaWebApp.Models;

namespace CinemaWebApp.Controllers
{
    public class FilmsController : Controller
    {
        private readonly CinemaContext _context;

        public FilmsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            return View(await _context.Films.ToListAsync());
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Description,Length,Price")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/VisaForestallningar
        public async Task<IActionResult> VisaForestallningar(int filmId)
        {
            var forestallningar = await _context.Föreställningar
                .Include(f => f.Salong)
                .Include(f => f.Film)
                .Where(f => f.FilmId == filmId)
                .ToListAsync();

            if (!forestallningar.Any())
            {
                return NotFound("Inga föreställningar hittades för denna film.");
            }

            ViewBag.FilmTitle = forestallningar.First().Film.Title; // För att visa filmtiteln i vy
            return View(forestallningar);
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
