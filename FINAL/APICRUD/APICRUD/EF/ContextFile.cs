using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using APICRUD.EF.Entities;

namespace APICRUD.EF
{
    public class ContextFile : DbContext

    {
        public ContextFile() : base()
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }


    }
}