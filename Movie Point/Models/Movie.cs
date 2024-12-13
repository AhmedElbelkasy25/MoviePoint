using Microsoft.AspNetCore.Mvc;
using Movie_Point.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace Movie_Point.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        //[Remote(Action: "SearchForMovie" , Controller:"Movie")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        
        public string ImgUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus { get; set; }
        public int NumOfWatch { get; set; } = 0;

        public int CinemaId { get; set; }
        public int CategoryId { get; set; }
        public Cinema Cinema { get; set; }
        public Category Category { get; set; }
        public List<Actor> Actors { get; set; }
        public List<ActorMovie> actorsMovies { get; set; }
        // public List<Order> Orders { get; set; }


    }
}
