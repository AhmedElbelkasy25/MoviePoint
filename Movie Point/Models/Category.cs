using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Length(3,15,ErrorMessage = "Name must be in 3 charechters to 15")]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
