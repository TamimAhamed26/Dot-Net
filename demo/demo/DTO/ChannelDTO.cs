using demo.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace demo.DTO
{
    public class ChannelDTO
    {

        
        [Required(ErrorMessage = "ChannelName is required")]
        [MaxLength(100, ErrorMessage = "ChannelName cannot exceed 100 characters")]
        public string ChannelName { get; set; }

        [DateValidation]
        public int EstablishedYear { get; set; }


        public int ChannelId { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }
    }
}






