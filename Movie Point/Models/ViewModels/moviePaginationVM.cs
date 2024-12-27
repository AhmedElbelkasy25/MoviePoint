namespace Movie_Point.Models.ViewModels
{
    public class moviePaginationVM
    {
        public int Pages { get; set; }
        public List<Movie> Movies { get; set; }
        public int PageNumber { get; set; }
    }
}
