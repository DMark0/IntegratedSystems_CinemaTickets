using CinemaTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<CinemaTicketsUsers> GetAll();
        CinemaTicketsUsers Get(string id);
        void Insert(CinemaTicketsUsers entity);
        void Update(CinemaTicketsUsers entity);
        void Delete(CinemaTicketsUsers entity);
    }
}
