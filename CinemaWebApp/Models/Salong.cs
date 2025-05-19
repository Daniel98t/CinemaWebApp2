using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.Models
{
    public class Salong
    {
        public int Id { get; set; } // Primärnyckel

        [Range(1, int.MaxValue)]
        public int Number { get; set; } // Salongens nummer

        [Range(1, 300)]
        public int Seats { get; set; } // Antal stolar i salongen
    }
}
