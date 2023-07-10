using Microsoft.EntityFrameworkCore;
using CinemaTickets.Domain.Models;
using CinemaTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTickets.Repository.Implementation
{
    public class CinemaTicketsRepository : ICinemaTicketsRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<CinemaTicketsFilm> entities;
        string errorMessage = string.Empty;

        public CinemaTicketsRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<CinemaTicketsFilm>();
        }

        public void Delete(CinemaTicketsFilm entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public CinemaTicketsFilm Get(Guid? id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<CinemaTicketsFilm> GetAll()
        {
            return entities.AsEnumerable();
        }

        public CinemaTicketsFilm GetByName(string name)
        {
            return entities.SingleOrDefault(s => s.Name == name);
        }

        public void Insert(CinemaTicketsFilm entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(CinemaTicketsFilm entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
