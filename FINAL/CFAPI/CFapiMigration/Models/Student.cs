using Newtonsoft.Json;
using System.Collections.Generic;

namespace CFapiMigration.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary key


        public string Name { get; set; }


        public int Age { get; set; }

       // [JsonIgnore]
        // Navigation property
        public virtual ICollection<Course> Courses { get; set; }
    }
}