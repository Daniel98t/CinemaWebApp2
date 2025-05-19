using System.ComponentModel.DataAnnotations;

namespace CinemaWebApp.Models
{
    public class Film
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Range(30, 300)]
        public int Length { get; set; }

        [Range(0, 500)]
        public decimal Price { get; set; }
    }
}
