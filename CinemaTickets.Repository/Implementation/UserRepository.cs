using Microsoft.EntityFrameworkCore;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTickets.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<CinemaTicketsUsers> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<CinemaTicketsUsers>();
        }
        public IEnumerable<CinemaTicketsUsers> GetAll()
        {
            return entities.AsEnumerable();
        }

        public CinemaTicketsUsers Get(string id)
        {
            return entities
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.filmsinshoppingcart)
                .Include("UserCart.filmsinshoppingcart.film")
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(CinemaTicketsUsers entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(CinemaTicketsUsers entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(CinemaTicketsUsers entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
