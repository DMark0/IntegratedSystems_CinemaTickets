using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinemaTickets.Domain.DTO
{
    public class MailDto
    {
        [Required]
        public String subject { get; set; }
        [Required]
        public String message { get; set; }
    }
}
