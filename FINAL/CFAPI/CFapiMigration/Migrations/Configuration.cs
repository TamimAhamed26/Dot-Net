namespace CFapiMigration.Migrations
{
    using CFapiMigration.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    //auto generated.after (i)Enable-Migrations (ii)Add-Migration InitialCreate

    internal sealed class Configuration : DbMigrationsConfiguration<CFapiMigration.Models.ContextFile>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }



        //seed values.(iii)Update-Database

        protected override void Seed(CFapiMigration.Models.ContextFile context)
        {
            var course1 = new Course { Title = "Mathematics", Credits = 3 };
            var course2 = new Course { Title = "Physics", Credits = 4 };

            var student1 = new Student { Name = "Tamim Ahamed", Age = 20 };
            var student2 = new Student { Name = "Rahim Karim", Age = 22 };

            student1.Courses = new List<Course> { course1, course2 };
            student2.Courses = new List<Course> { course1 };

            context.Students.AddOrUpdate(student1, student2);
            context.Courses.AddOrUpdate(course1, course2);

            context.SaveChanges();
        }
    }
}
