using prac.EF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace prac.EF
{
    public class NewsContext : DbContext
    {

        public DbSet<News> News { get; set; }   
    }
}