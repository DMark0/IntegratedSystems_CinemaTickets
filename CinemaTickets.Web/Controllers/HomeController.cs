using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CinemaTickets.Domain.DTO;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Domain.Models;
using CinemaTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaTickets.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;  
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact([Bind("subject", "message")] MailDto item)
        {
            if (ModelState.IsValid)
            {
                String sub = item.subject;
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CinemaTicketsUsers user = _userService.Get(userId);
            MailAddress from = new MailAddress("integriraniproekt@outlook.com");
            MailAddress to = new MailAddress("integriraniproekt@outlook.com");
            MailMessage mail = new MailMessage(to, from);
            mail.Subject = item.subject;
            mail.Body = item.message;
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.Timeout = 1000000;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            NetworkCredential nc = new NetworkCredential("integriraniproekt@outlook.com", "CinemaTicketsApp123!");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mail);
            _logger.LogInformation("Korisnikot prakja poraka za kontakt");
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
