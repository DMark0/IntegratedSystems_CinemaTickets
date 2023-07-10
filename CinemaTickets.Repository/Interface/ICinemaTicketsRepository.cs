using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Repository.Interface
{
    public interface ICinemaTicketsRepository
    {
        IEnumerable<CinemaTicketsFilm> GetAll();
        CinemaTicketsFilm Get(Guid? id);
        void Insert(CinemaTicketsFilm entity);
        void Update(CinemaTicketsFilm entity);
        void Delete(CinemaTicketsFilm entity);
        CinemaTicketsFilm GetByName(string name);
    }
}
