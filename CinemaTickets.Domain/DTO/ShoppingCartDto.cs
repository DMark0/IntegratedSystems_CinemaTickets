using CinemaTickets.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<FilminShoppingCart> filmsinShoppingCarts { get; set; }
        public double TotalPrice { get; set; }
    }
}
