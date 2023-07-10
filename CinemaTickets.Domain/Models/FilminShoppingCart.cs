using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Domain
{
    public class FilminShoppingCart: BaseEntity
    {//ticketinshopingcart
        public Guid FilmId { get; set; }
        public Guid ShoppingCartId { get; set; }
        public CinemaTicketsFilm film { get; set; }//ticket
        [Range(1, 5)]
        public int Kvalitet { get; set; }
        public ShoppingCart shoppingcart { get; set; }
    }

}
