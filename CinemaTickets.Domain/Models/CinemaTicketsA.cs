using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Domain.Models
{
    public class CinemaTicketsA
    {
        public String Title { get; set; }
        public int Year { get; set; }
        public String Rated { get; set; }
        public String imdbRating { get; set; }
        public String imdbVotes { get; set; }
        public String Released { get; set; }
        public String Runtime { get; set; }
        public String Genre { get; set; }
        public String Director { get; set; }
        public String Writer { get; set; }
        public String Actors { get; set; }
        public String Plot { get; set; }
        public String Language { get; set; }
        public String Country { get; set; }
        public String Awards { get; set; }
        public String Poster { get; set; }
        public bool Added { get; set; }
    }
}
