using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APICRUD.EF.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; }

        [JsonIgnore] //OR HAVE TO USE DTO 
        public Author Author { get; set; }

    }
}