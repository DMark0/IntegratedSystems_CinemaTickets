using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public CinemaTicketsFilm SelectedFilm { get; set; }
        public Guid FilmId { get; set; }
        public int Kvalitet { get; set; }
    }
}
