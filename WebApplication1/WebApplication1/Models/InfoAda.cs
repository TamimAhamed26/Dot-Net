using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class InfoAda
    {
        public string Name { get; set; } //Auto-implemented property not full property

        public string Id { get; set; }

        public int Age { get; set; }


        /*Full property
        private string _address; 
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        */

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public List<Education> EducationList { get; set; }
        public List<WorkExperience> WorkExperienceList { get; set; }
        public List<string> Skills { get; set; }
        public List<Project> Projects { get; set; }
        public List<string> Certifications { get; set; }
        public List<string> Languages { get; set; }
    }

    // Nested model classes for detailed CV sections
    public class Education
    {
        public string Degree { get; set; }
        public string University { get; set; }
        public string Year { get; set; }
        public string Result { get; set; }
    }

    public class WorkExperience
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string Responsibilities { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
    }
}