using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models.ViewModels
{
    public class EmailVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="User Name Or Email")]
        [Length(6,40)]
        public string Account { get; set; }
    }
}
