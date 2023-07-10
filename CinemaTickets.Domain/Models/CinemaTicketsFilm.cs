using CinemaTickets.Domain.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinemaTickets.Domain.Models
{
    public class CinemaTicketsFilm : BaseEntity
    {
        /*[Required]
        public string Ticketfor { get; set; }
        [Required]
        public string mesto { get; set; }
        [Required]
        public DateTime datum { get; set; }
        public string zanr { get; set; }
        public int Ticketprice { get; set; }
        public virtual ICollection<TicketsinShoppingCart> ticketsinshoppingcart { get; set; }
        public virtual ICollection<TicketInOrder> Orders { get; set; }*/
        //[Key]//u aud tiket
        //public int Id { get; set; }

        public string Name { get; set; }//ticketfor
        public string Description { get; set; }//mesto
        [DataType(DataType.Date)]
        public DateTime datum { get; set; }
        public string zanr { get; set; }
        public int Price { get; set; }//ticketprice
        public string ImageURL { get; set; }
        public virtual ICollection<FilminShoppingCart> filmsinshoppingcart { get; set; }//ticketsinshoppingcart
        public virtual ICollection<FilmInOrder> Orders { get; set; }
        public string trailerURL { get; set; }

        //public CinemaTicketsCategory CinemaTicketsCategory { get; set; }

        //Relationships
        //public List<Actor_CinemaTickets> Actors_CinemaTicketss { get; set; }
    }
}
