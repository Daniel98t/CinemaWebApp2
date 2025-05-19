using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaWebApp.Models
{
    public class Bokning
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Föreställning))]
        [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig föreställning.")]
        public int FöreställningId { get; set; }

        [Required]
        public Föreställning Föreställning { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig plats.")]
        public int SeatNumber { get; set; }

        [Required]
        [StringLength(36, ErrorMessage = "Bokningsnumret måste vara en giltig GUID.")]
        public string BookingNumber { get; set; } = null!;

        [Required(ErrorMessage = "Ange ditt namn.")]
        [StringLength(50, ErrorMessage = "Namnet får inte vara längre än 50 tecken.")]
        public string CustomerName { get; set; } = null!;

        [Required(ErrorMessage = "Ange din e-postadress.")]
        [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
        public string CustomerEmail { get; set; } = null!;
    }
}
