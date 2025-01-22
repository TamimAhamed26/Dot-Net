using demo.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace demo.DTO
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }

        [Required(ErrorMessage = "ChannelName is required")]
        [MaxLength(150, ErrorMessage = "ChannelName cannot exceed 100 characters")]
        public string ProgramName { get; set; }

        [Required(ErrorMessage = "TRPScore is required")]
        [Range(0.0, 10.0, ErrorMessage = "TRPScore must be between 0.0 and 10.0")]
        public decimal TRPScore { get; set; }

        [Required(ErrorMessage = "ChannelId is required")]
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "AirTime is required")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$", ErrorMessage = "AirTime must be a valid time in HH:mm:ss format")]
        public System.TimeSpan AirTime { get; set; }
    }
}