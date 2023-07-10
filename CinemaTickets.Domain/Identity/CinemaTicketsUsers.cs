using Microsoft.AspNetCore.Identity;
using CinemaTickets.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTickets.Domain.Identity
{
    public class CinemaTicketsUsers: IdentityUser
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public virtual ShoppingCart UserCart { get; set; }
    }
}
