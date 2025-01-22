using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APICRUD.EF.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        [JsonIgnore] //OR HAVE TO USE DTO 
        public List<Book> Book { get; set; }
    }
}