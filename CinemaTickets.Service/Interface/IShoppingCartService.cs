using CinemaTickets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        void deleteTicketFromShoppingCart(String userId, Guid id);
        bool orderNow(string userId);
    }
}
