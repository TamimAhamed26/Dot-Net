using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class LayerContext : DbContext

    {
        public LayerContext() : base() { }

        public DbSet<News> NewsS { get; set; }
    }
}