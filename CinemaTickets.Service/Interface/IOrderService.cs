using CinemaTickets.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetAllOrdersByUser(string userId);
        Order GetOrderDetails(BaseEntity model);

        Order GetOrderDetailsById(Guid id);
    }
}
