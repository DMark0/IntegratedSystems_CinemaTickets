using CinemaTickets.Domain.DTO;
using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface ICinemaTicketsService
    {
        List<CinemaTicketsFilm> GetAllTickets();
        CinemaTicketsFilm GetDetailsForTicket(Guid? id);
        CinemaTicketsFilm GetCinemaTicketsByName(string name);
        void CreateNewTicket(CinemaTicketsFilm t);
        void UpdeteExistingTicket(CinemaTicketsFilm t);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
