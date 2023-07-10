using CinemaTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface IUserService
    {
        CinemaTicketsUsers Get(string id);

    }
}
