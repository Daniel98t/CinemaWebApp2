using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaWebApp.Controllers
{
    public class BokningsController : Controller
    {
        private readonly CinemaContext _context;

        public BokningsController(CinemaContext context)
        {
            _context = context;
        }

        // Sök bokning - GET
        public IActionResult Search()
        {
            return View();
        }

        // Sök bokning - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string bookingNumber)
        {
            if (string.IsNullOrWhiteSpace(bookingNumber))
            {
                ModelState.AddModelError("", "Vänligen ange ett giltigt bokningsnummer.");
                return View();
            }

            var bokning = _context.Bokningar
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Film)
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Salong)
                .FirstOrDefault(b => b.BookingNumber == bookingNumber);

            if (bokning == null)
            {
                ModelState.AddModelError("", "Bokning med angivet bokningsnummer hittades inte.");
                return View();
            }

            return View("Details", bokning);
        }

        // Visa alla bokningar
        public async Task<IActionResult> Index()
        {
            var bokningar = await _context.Bokningar
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Film)
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Salong)
                .ToListAsync();
            return View(bokningar);
        }

        // Visa lediga platser för en föreställning
        public IActionResult LedigaPlatser(int föreställningId)
        {
            var föreställning = _context.Föreställningar
                .Include(f => f.Salong)
                .Include(f => f.Film)
                .Include(f => f.Bokningar)
                .FirstOrDefault(f => f.Id == föreställningId);

            if (föreställning == null || föreställning.Salong == null)
            {
                return NotFound("Föreställning eller salong hittades inte.");
            }

            var bokadePlatser = föreställning.Bokningar
                .Select(b => b.SeatNumber)
                .ToList();

            var ledigaPlatser = Enumerable.Range(1, föreställning.Salong.Seats)
                .Select(seat => new SelectListItem
                {
                    Value = seat.ToString(),
                    Text = $"Plats {seat}",
                    Disabled = bokadePlatser.Contains(seat)
                })
                .ToList();

            ViewBag.LedigaPlatser = ledigaPlatser;
            ViewBag.CurrentFöreställningId = föreställning.Id; // Lägger till nuvarande föreställningId
            return View(föreställning);
        }

        // Skapa bokning - GET
        public IActionResult Create(int? Id)
        {
            if (Id == null)
            {
                return NotFound("Föreställningens ID kunde inte hittas.");
            }

            var föreställning = _context.Föreställningar
                .Include(f => f.Salong)
                .Include(f => f.Film)
                .FirstOrDefault(f => f.Id == Id);

            if (föreställning == null)
            {
                return NotFound("Föreställning kunde inte hittas.");
            }

            // Kontrollera att Salong och Film inte är null
            if (föreställning.Salong == null || föreställning.Film == null)
            {
                return NotFound("Kopplad salong eller film kunde inte hittas.");
            }

            // Skapa en SelectList för FöreställningId
            ViewBag.FöreställningId = new SelectList(
                new List<object> { new { Id = föreställning.Id, Title = föreställning.Film.Title } },
                "Id",
                "Title",
                föreställning.Id
            );

            // Kontrollera att Salong.Seats är ett giltigt värde
            if (föreställning.Salong.Seats <= 0)
            {
                return BadRequest("Antalet platser i salongen är ogiltigt.");
            }

            // Skapa en lista över tillgängliga platser
            ViewBag.Platser = Enumerable.Range(1, föreställning.Salong.Seats).ToList();

            // Ladda bokade platser
            ViewBag.BokadePlatser = _context.Bokningar
                .Where(b => b.FöreställningId == Id)
                .Select(b => b.SeatNumber)
                .ToList();

            ViewBag.CurrentFöreställningId = Id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int Id, int seatNumber, string customerName, string customerEmail)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Formuläret är inte giltigt.");
                return View();
            }

            var föreställning = _context.Föreställningar
                .Include(f => f.Salong)
                .FirstOrDefault(f => f.Id == Id);

            if (föreställning == null)
            {
                ModelState.AddModelError("", $"Föreställning med ID {Id} hittades inte.");
                return View();
            }

            if (föreställning.Salong == null)
            {
                ModelState.AddModelError("", "Salong är inte kopplad till föreställningen.");
                return View();
            }

            var platsBokad = _context.Bokningar
                .Any(b => b.FöreställningId == Id && b.SeatNumber == seatNumber);

            if (platsBokad)
            {
                ModelState.AddModelError("", "Platsen är redan bokad.");
                return View();
            }

            var bokning = new Bokning
            {
                FöreställningId = Id,
                SeatNumber = seatNumber,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                BookingNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper() // Genererar ett kortare nummer
            };

            _context.Bokningar.Add(bokning);
            _context.SaveChanges();

            return RedirectToAction("Bekräftelse", new { id = bokning.Id });
        }


        // Visa bokningsbekräftelse
        public IActionResult Bekräftelse(int id)
        {
            var bokning = _context.Bokningar
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Film)
                .Include(b => b.Föreställning)
                .ThenInclude(f => f.Salong)
                .FirstOrDefault(b => b.Id == id);

            if (bokning == null || bokning.Föreställning == null || bokning.Föreställning.Salong == null)
            {
                return NotFound("Bokning, föreställning eller salong hittades inte.");
            }

            return View(bokning);
        }
    }
}
