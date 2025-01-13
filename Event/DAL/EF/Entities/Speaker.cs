using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class Speaker
    {
        public int SpeakerId { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
