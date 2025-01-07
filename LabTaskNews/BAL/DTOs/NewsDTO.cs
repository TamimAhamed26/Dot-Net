using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class NewsDTO 
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
