using Microsoft.AspNetCore.Mvc;
using CinemaTickets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using CinemaTickets.Domain.Models;
using CinemaTickets.Service.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CinemaTickets.Web.Controllers
{
    [Authorize(Roles = "Administrators, Users")]
    public class FilmInf : Controller
    {

        private readonly ICinemaTicketsService _movieService;
        private readonly ILogger<FilmInf> _logger;

        public FilmInf(ILogger<FilmInf> logger, ICinemaTicketsService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult CinemaTickets()
        {
            return View();
        }

        // POST: Ucenicis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CinemaTickets([Bind("Name")] CinemaTicketsDto item )
        {
            if (ModelState.IsValid)
            {
                String name = item.Name;
                
            }
            String namee = item.Name;
            HttpClient client = new HttpClient();
            string URL = "http://www.omdbapi.com/?t="+item.Name+"&apikey=293abe22";
            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<CinemaTicketsA>().Result;
            data.Added = this._movieService.GetCinemaTicketsByName(data.Title) != null ? true: false;
            _logger.LogInformation("Korisnikot prebaruva informacii za nekoj film");
            return View("Index", data);
        }

        [HttpGet]
        public ActionResult CinemaTicketsDetails(string name, bool added)
        {
            HttpClient client = new HttpClient();
            string URL = "http://www.omdbapi.com/?t=" + name + "&apikey=293abe22";
            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<CinemaTicketsA>().Result;
            data.Added = added;
            _logger.LogInformation("Korisnikot gleda informacii za nekoj film");
            return View("Index", data);
        }

    }
}
