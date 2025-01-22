using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class SpeakerDTO
    {

        public int SpeakerId { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Spekaer name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Spekaer name must be between 2 and 100 characters.")]
        public string Topic { get; set; }
        public int EventId { get; set; }
       



    }



}
