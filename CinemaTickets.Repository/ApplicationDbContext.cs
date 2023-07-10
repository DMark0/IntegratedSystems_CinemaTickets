using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Repository
{
    public class ApplicationDbContext : IdentityDbContext<CinemaTicketsUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<CinemaTicketsFilm> Films { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<FilminShoppingCart> FilminShoppingCarts { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }   

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Conventious.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(builder);

            builder.Entity<CinemaTicketsFilm>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Order>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            /*builder.Entity<TicketsinShoppingCart>()
                .HasKey(z => new
                {
                    z.TicketId,
                    z.ShoppingCartId
                });*/
            builder.Entity<FilminShoppingCart>()
                .HasOne(z => z.film)
                .WithMany(z => z.filmsinshoppingcart)
                .HasForeignKey(z => z.ShoppingCartId);
            builder.Entity<FilminShoppingCart>()
                .HasOne(z => z.shoppingcart)
                .WithMany(z => z.filmsinshoppingcart)
                .HasForeignKey(z => z.FilmId);
            builder.Entity<ShoppingCart>()
                .HasOne<CinemaTicketsUsers>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);
            /*builder.Entity<TicketInOrder>()
                .HasKey(z => new
                {
                    z.TicketId,
                    z.OrderId
                });*/
            builder.Entity<FilmInOrder>()
                .HasOne(z => z.SelectedFilm)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.FilmId);
            builder.Entity<FilmInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.FilmInOrders)
                .HasForeignKey(z => z.OrderId);
        }


    }
}
