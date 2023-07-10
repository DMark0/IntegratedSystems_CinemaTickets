using Microsoft.EntityFrameworkCore;
using CinemaTickets.Domain.Domain;
using CinemaTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTickets.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z=>z.FilmInOrders)
                .Include(z=>z.User)
                .Include("FilmInOrders.SelectedFilm")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
                .Include(z => z.FilmInOrders)
                .Include(z => z.User)
                .Include("FilmInOrders.SelectedFilm")
                .SingleOrDefaultAsync(z=>z.Id==model.Id).Result;
        }

        public Order GetOrderDetailsById(Guid id)
        {
            return entities
                .Include(z => z.FilmInOrders)
                .Include(z => z.User)
                .Include("FilmInOrders.SelectedFilm")
                .SingleOrDefaultAsync(z => z.Id == id).Result;
        }

        public List<Order> GetAllOrdersByUser(string userId)
        {
            return entities
                .Where( z => z.UserId == userId)
                .Include(z => z.FilmInOrders)
                .Include(z => z.User)
                .Include("FilmInOrders.SelectedFilm")
                .ToListAsync().Result;
        }
    }
}
