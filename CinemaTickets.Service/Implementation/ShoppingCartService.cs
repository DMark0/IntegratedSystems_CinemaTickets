using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.DTO;
using CinemaTickets.Repository.Interface;
using CinemaTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTickets.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;
        private readonly IRepository<FilmInOrder> _filmsInOrderRepository;
        private readonly IUserRepository _userRepository;
        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<EmailMessage> mailRepository, IRepository<Order> orderRepository, IRepository<FilmInOrder> filmsInOrderRepository, IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _mailRepository = mailRepository;
            _filmsInOrderRepository = filmsInOrderRepository;
            _userRepository = userRepository;
        }

        public void deleteTicketFromShoppingCart(String userId, Guid id)
        {
            if(!string.IsNullOrEmpty(userId)&&id!=null)
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userShoppingCart = loggedInUser.UserCart;
                var producttodelete = userShoppingCart.filmsinshoppingcart.Where(z => z.FilmId.Equals(id)).FirstOrDefault();
                userShoppingCart.filmsinshoppingcart.Remove(producttodelete);
                this._shoppingCartRepository.Update(userShoppingCart);
            }
            
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);
            var userShoppingCart = loggedInUser.UserCart;
            var filmPrice = userShoppingCart.filmsinshoppingcart.Select(z => new
            {
                filmPrice = z.film.Price,
                Kvalitet = z.Kvalitet

            }).ToList();
            double totalPrice = 0;
            foreach (var item in filmPrice)
            {
                totalPrice = totalPrice + item.filmPrice * item.Kvalitet;
            }
            ShoppingCartDto shopingcartdtoitem = new ShoppingCartDto
            {
                filmsinShoppingCarts = userShoppingCart.filmsinshoppingcart.ToList(),
                TotalPrice = totalPrice
            };
            return shopingcartdtoitem;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                    var loggedInUser = this._userRepository.Get(userId);
                    var userShoppingCart = loggedInUser.UserCart;
                EmailMessage emailMessage = new EmailMessage();
                emailMessage.emailsendTo = loggedInUser.Email;
                emailMessage.Subject = "Succesfull order";
                emailMessage.Status = false;
                    Order orderItem = new Order
                    {
                       Id = Guid.NewGuid(),
                       UserId = userId,
                       User = loggedInUser,
                       Created = DateTime.Now
                     };
                     this._orderRepository.Insert(orderItem);
                     List<FilmInOrder> filmInOrders = new List<FilmInOrder>();
                filmInOrders = userShoppingCart.filmsinshoppingcart.Select(z => new FilmInOrder
                {
                    Id = Guid.NewGuid(),
                         OrderId = orderItem.Id,
                    FilmId = z.film.Id,
                    SelectedFilm = z.film,
                    UserOrder = orderItem,
                    Kvalitet=z.Kvalitet
                }).ToList();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Order is completed it contains");////////////////////////tujjjjjj
                var totalPrice = 0;
                for (int i = 1; i <= filmInOrders.Count(); i++)
                {
                    var item = filmInOrders[i - 1];
                    totalPrice = item.Kvalitet * item.SelectedFilm.Price;
                    sb.AppendLine(i.ToString()+" "+item.SelectedFilm.Name + "cena" + item.SelectedFilm.Price + "kvalitet" + item.Kvalitet);
                }
                sb.AppendLine("total price" + totalPrice.ToString());
                emailMessage.Content = sb.ToString();
                    foreach (var item in filmInOrders)
                     {
                        this._filmsInOrderRepository.Insert(item);
                    }
                    loggedInUser.UserCart.filmsinshoppingcart.Clear();
                this._mailRepository.Insert(emailMessage);
                    this._userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }
    }
}
