using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CinemaTickets.Domain.Identity;
using System.Security.Claims;
using CinemaTickets.Service.Interface;
using System;
using GemBox.Document;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CinemaTickets.Web.Controllers
{
    [Authorize(Roles = "Administrators, Users")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<CinemaTicketsUsers> _userManager;

        public OrderController(IOrderService orderService, UserManager<CinemaTicketsUsers> userManager)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._orderService.GetAllOrdersByUser(userId));
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            var order = this._orderService.GetOrderDetailsById(id); 

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{CustomerDetails}}", order.User.FirstName + " " + order.User.LastName);

            var totalPrice = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.FilmInOrders)
            {
                sb.AppendLine("CinemaTickets name: " + item.SelectedFilm.Name + " Quantity: " + item.Kvalitet + " Total price:" + item.SelectedFilm.Price * item.Kvalitet + "$");
                totalPrice += item.SelectedFilm.Price * item.Kvalitet;
            }

            document.Content.Replace("{{CinemaTicketssInOrder}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");
            document.Content.Replace("{{OrderDate}}", order.Created.ToShortDateString());

            var stream = new MemoryStream();

            document.Save(stream, new DocxSaveOptions());

            return File(stream.ToArray(), new DocxSaveOptions().ContentType, "ExportInvoice.docx");
        }
    }
}
