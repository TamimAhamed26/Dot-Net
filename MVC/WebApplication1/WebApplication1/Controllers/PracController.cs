using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;//importing the namespace of InfoAda class

namespace WebApplication1.Controllers
{
    public class PracController : Controller
    {
        public ActionResult List()
        {
            /* 1.learn about var and dynamic    
             dynamic h = 12;
             h = "2323";  
             No error occurs because the variable h is dynamically typed, 
             meaning its type can change at runtime.

             dynamic h = 12;
             h.ghds = "2323"; 
             This will cause a runtime error because  attempting to set a property ghds on an integer, 
             and integers (int) do not have a property called ghds. While the code compiles, when it runs,
             the runtime cannot resolve ghds as a valid property of the integer type, which causes the error.
            */

            /* 
            var h = 12;
            h.ghds = "23s23";
            This will cause a compile-time error because the variable h is statically typed,
            */

            /* 2.learn ViewBag
            int a = 10;
            int b = 20;
            int c = a + b;

            ViewBag.Y = c; // ViewBag is a dynamic object, it can store any type of data;
            */

            /*
               string[] names = new string[3];
               names[0] = "John";
               names[1] = "Tamim";
               names[2] = "Ahamed";

               string[] ids = new string[3];
               ids[0] = "22-12345-1";
               ids[1] = "22-46923-1";
               ids[2] = "22-67890-1";

               int[] ages = new int[3];
               ages[0] = 25;
               ages[1] = 22;
               ages[2] = 30;

               ViewBag.Names = names; 
               ViewBag.Ids = ids; 
               ViewBag.Ages = ages; 
            */

            //3. learn about compact object initialization and passing object to view

            InfoAda S1 = new InfoAda() //creating instance of InfoAda class with annomous constructor
            {
                Name = "Jodhn",
                Id = "22-12343-1",
                Age = 25
            };

            //return View(S1); //PASS THE INSTANCE S1/ if the view folder is prac & the view file is named list
            return View("~/Views/Uname/error1.cshtml",S1); //here the view folder is different from Controller(prac) & and the view file name(error1) is different 
        }                                               // from the action method(list) ;so,we have to give the abosulute path
                                                        //but the call for both methods will be same;
        public ActionResult Multi()
        {
            List<InfoAda> students = new List<InfoAda>(); //creating a list of InfoAda class
            for (int i = 0; i <10; i++)
            {
                InfoAda S1 = new InfoAda() //creating instance of InfoAda class with annomous constructor
                {
                    Name = "Jodhn",
                    Id = "22-12343-" + (i + 1),
                    Age = 25
                };
                students.Add(S1); //adding the instance to the list
            }
            return View(students);

        }
        public ActionResult Cv()
        {
            var cv = new InfoAda
            {
                Name = "Tamim",
                Email = "tamim@example.com",
                Phone = "+88012345670",
                Address = "123 Main Street, Dinajpur",

                // Education details
                EducationList = new List<Education>
                {
                    new Education { Degree = "BSc in Computer Science", University = "AIUB", Year = "2022 - 2025", Result = "CGPA 3.8/4.0" },
                    new Education { Degree = "HSC", University = "Dinajpur Govt City College", Year = "2018 - 2020", Result = "GPA 5.0/5.0" },
                    new Education { Degree = "SSC", University = "Dinajpur Zilla School", Year = "2016 - 2018", Result = "GPA 5.0/5.0" },
                    new Education { Degree = "JSC", University = "Dinajpur Zilla School", Year = "2016", Result = "GPA 5.0/5.0" }

                },

                // Work experience
                WorkExperienceList = new List<WorkExperience>
                {
                    new WorkExperience { Position = "Software Developer", Company = "XYZ Company", Year = "2026 - Present", Responsibilities = "Developing web applications and maintaining databases" },
                    new WorkExperience { Position = "Intern", Company = "ABC Tech", Year = "2025", Responsibilities = "Assisted in software development and testing" }
                },

                // Skills
                Skills = new List<string> { "C#", "ASP.NET", "SQL Server", "JavaScript" },

                // Projects
                Projects = new List<Project>
                {
                    new Project { Name = "Project Management System", Description = "A web application for managing projects", Technologies = "ASP.NET, SQL Server" }
                },

                // Certifications
                Certifications = new List<string> { "Microsoft Certified: Azure Fundamentals", "Certified ScrumMaster" },

                // Languages
                Languages = new List<string> { "English - Fluent", "Bengali - Native" }
            };

            return View(cv); // Passing the populated object to the view
        }




    }
}