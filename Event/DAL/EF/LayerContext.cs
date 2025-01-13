using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class LayerContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
    }
}
