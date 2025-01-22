using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class EventDTO
    {
        public int EventId { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Event name must be between 2 and 100 characters.")]
        public string Name { get; set; }
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Event name must be between 2 and 200 characters.")]
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
