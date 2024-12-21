using System.ComponentModel.DataAnnotations;

namespace Movie_Point.Models.ViewModels
{
    public class ForgetPassordVM
    {
        public int Id { get; set; }
        public string Account { get; set; }
        [Required]
        [Length(6, 50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassord { get; set; }
    }
}
