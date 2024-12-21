using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="UserName Or Email")]
        public string Account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
