using CinemaTickets.Domain.Identity;
using CinemaTickets.Repository.Interface;
using CinemaTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Implementation
{
    public class IUserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;
        public IUserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public CinemaTicketsUsers Get(string id)
        {
            return _userRepository.Get(id);
        }
    }
}
