using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CinemaTickets.Service.Interface;
using CinemaTickets.Domain.Models;
using CinemaTickets.Domain.DTO;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Repository.Implementation;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using ExcelDataReader;
using CinemaTickets.Domain.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CinemaTickets.Web.Controllers
{
    [Authorize(Roles = "Administrators, Users")]
    public class CinemaTicketsController : Controller
    {
        private readonly ICinemaTicketsService _movieService;
        private readonly IUserService _userService;

        public CinemaTicketsController(ICinemaTicketsService movieService, IUserService userService)
        {
            _movieService = movieService;
            _userService = userService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var allCinemaTicketss = _movieService.GetAllTickets();
            return View(allCinemaTicketss);
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = this._movieService.GetDetailsForTicket(id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        [Authorize(Roles = "Administrators")]
        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrators")]
        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description, datum, zanr, Price, ImageURL, trailerURL")] CinemaTicketsFilm film)
        {
            if (ModelState.IsValid)
            {
                this._movieService.CreateNewTicket(film);
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }
        public IActionResult AddTicketToCard(Guid? id)
        {
            var model = this._movieService.GetShoppingCartInfo(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCard([Bind("FilmId", "Kvalitet")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._movieService.AddToShoppingCart(item, userId);
            CinemaTicketsUsers user = _userService.Get(userId);
            var accountSID = "ACcb5ff2b20332747e2b91ec16438b1b44";
            var authToken = "1222ad496fe284d354ec792d70a9accd";
            TwilioClient.Init(accountSID, authToken);
            var tonum = user.PhoneNumber;
            var from = "+19786523857";
            if (result)
            {
                var message = MessageResource.Create(
                body: "New film is successfully added to your card. Check it!",
                from: new Twilio.Types.PhoneNumber("+1 978 652 3857"),
                to: new Twilio.Types.PhoneNumber(user.PhoneNumber));
                //var message = MessageResource.Create(to: new Twilio.Types.PhoneNumber(user.PhoneNumber), from: new Twilio.Types.PhoneNumber("+19786523857"), body: "New film is successfully added to your card. Check it!");

                //return Content(message.Sid);
                return RedirectToAction("Index", "CinemaTickets");
            }
            return View(item);
        }

        [Authorize(Roles = "Administrators")]
        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = this._movieService.GetDetailsForTicket(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        [Authorize(Roles = "Administrators")]
        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Description, datum, zanr, Price, ImageURL, trailerURL")] CinemaTicketsFilm film)
        {
            CinemaTicketsFilm movie = _movieService.GetDetailsForTicket(id);

            if (movie == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Name = film.Name;
                    movie.Description = film.Description;
                    movie.datum = film.datum;
                    movie.zanr = film.zanr;
                    movie.Price = film.Price;
                    movie.ImageURL = film.ImageURL;
                    movie.trailerURL = film.trailerURL;
                    this._movieService.UpdeteExistingTicket(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        [Authorize(Roles = "Administrators")]
        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = this._movieService.GetDetailsForTicket(id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        [Authorize(Roles = "Administrators")]
        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._movieService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return this._movieService.GetDetailsForTicket(id) != null;
        }

        [Authorize(Roles = "Administrators")]
        public IActionResult ImportCinemaTicketss(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<CinemaTicketsFilm> movies = getAllCinemaTicketssFromFile(file.FileName);

            foreach (CinemaTicketsFilm movie in movies)
            {
                if (this._movieService.GetCinemaTicketsByName(movie.Name) == null)
                {
                    this._movieService.CreateNewTicket(movie);
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private List<CinemaTicketsFilm> getAllCinemaTicketssFromFile(string fileName)
        {
            List<CinemaTicketsFilm> movies = new List<CinemaTicketsFilm>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if(reader.GetValue(0) != null && reader.GetValue(1) != null && reader.GetValue(2) != null 
                            && reader.GetValue(3) != null && reader.GetValue(4) != null && reader.GetValue(5) != null)
                        {
                            movies.Add(new Domain.Models.CinemaTicketsFilm
                            {
                                Name = reader.GetValue(0).ToString(),
                                Description = reader.GetValue(1).ToString(),
                                datum = (DateTime)reader.GetValue(2),
                                zanr = reader.GetValue(3).ToString(),
                                Price = int.Parse(reader.GetValue(4).ToString()),
                                ImageURL = reader.GetValue(5).ToString()
                            });
                        }
                        
                    }
                }
            }

            return movies;
        }

        [Authorize(Roles = "Administrators")]
        public IActionResult AddCinemaTicketsFromApi(string title, string description, string genre, string image)
        {
            CinemaTicketsFilm film = new CinemaTicketsFilm
            {
                Name = title,
                Description = description,
                datum = DateTime.Now,
                zanr = genre,
                ImageURL = image
            };
            this._movieService.CreateNewTicket(film);
            return RedirectToAction(nameof(Index));
        }
    }
}
