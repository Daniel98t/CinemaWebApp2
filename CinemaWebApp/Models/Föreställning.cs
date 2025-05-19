using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaWebApp.Models
{
    public class Föreställning
    {
        public int Id { get; set; } // Primärnyckel

        [ForeignKey("Film")]
        public int FilmId { get; set; } // Utländsk nyckel till Film
        public Film Film { get; set; } = null!; // Navigationsproperty

        [ForeignKey("Salong")]
        public int SalongId { get; set; } // Utländsk nyckel till Salong
        public Salong Salong { get; set; } = null!; // Navigationsproperty

        public DateTime Time { get; set; } // Tid och datum för föreställningen
        public ICollection<Bokning>? Bokningar { get; set; }

    }
}
