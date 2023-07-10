using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Service.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaTickets.Web.Controllers
{
    [Authorize(Roles = "Administrators, Users")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shopingCartService;
        private readonly UserManager<CinemaTicketsUsers> _userManager;


        public ShoppingCartController(IShoppingCartService shopingCartService, UserManager<CinemaTicketsUsers> userManager)
        {
            _shopingCartService = shopingCartService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //var allTickets = userShoppingCart.ticketsinshoppingcart.Select(z => z.ticket).ToList();
            return View(this._shopingCartService.getShoppingCartInfo(userId));
        }
        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            this._shopingCartService.deleteTicketFromShoppingCart(userId, id);
            return RedirectToAction("Index", "ShoppingCart");
        }
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shopingCartService.getShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.OrderNow();

                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult CreateOrder()
        {
            this.OrderNow();
            return RedirectToAction("Index", "ShoppingCart"); ;
        }
        private Boolean OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._shopingCartService.orderNow(userId);
            return result;
        }
    }
}
