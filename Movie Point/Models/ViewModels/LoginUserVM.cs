using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models.ViewModels
{
    public class LoginUserVM
    {
        public int Id { get; set; }
        [Required]
        [Length(5, 50)]
        public string UserName { get; set; }
        [Required]
        [Length(3, 50)]
        public string Name { get; set; }
        [Required]
        [Length(6, 50)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Length(6,50)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name="ConfirmPassword")]
        public string conPassword { get; set; }
        public string ImgUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}
