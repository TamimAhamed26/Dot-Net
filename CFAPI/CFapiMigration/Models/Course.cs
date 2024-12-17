using Newtonsoft.Json;
using System.Collections.Generic;

namespace CFapiMigration.Models
{
    public class Course
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; }
        public int Credits { get; set; }

        //[JsonIgnore]
        // Navigation property
        public virtual ICollection<Student> Students { get; set; }
    }

}