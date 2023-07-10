using CinemaTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Domain
{
    public class Order:BaseEntity
    {
        public string UserId { get; set; }
        public CinemaTicketsUsers User { get; set; }
        public DateTime Created { get; set; }   
        public virtual IList<FilmInOrder> FilmInOrders { get; set; }//TicketinOrder
    }
}
