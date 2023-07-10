using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Domain
{
    public class FilmInOrder : BaseEntity
    {
        public Guid FilmId { get; set; }
        public Order UserOrder { get; set; }
        public Guid OrderId { get; set; }
        public CinemaTicketsFilm SelectedFilm { get; set; }//SelectedTicket
        [Range(1, 5)]
        public int Kvalitet { get; set; }
    }
}
