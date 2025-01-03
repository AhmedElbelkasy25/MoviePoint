using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models
{
    public class RequestCinema
    {
        public int Id { get; set; }
        [Required]
        [Length(3, 25, ErrorMessage = "three charechters or more")]
        public string Name { get; set; }
        [Required]
        [Length(3, 25, ErrorMessage = "three charechters or more")]
        public string Description { get; set; }
        [Required]
        public string CinemaLogo { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
