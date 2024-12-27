﻿namespace Movie_Point.Models
{
    public class Cart
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int Count { get; set; }
        public DateTime Time { get; set; }
    }
}
