namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.EF.Entities.LayerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

   
protected override void Seed(DAL.EF.Entities.LayerContext context)
        {
            /*
            // Adding sample events
            for (int i = 1; i <= 5; i++)
            {
                context.Events.Add(new DAL.EF.Entities.Event()
                {
                    Name = "Event " + i,
                    Location = "Location " + i,
                    StartDate = DateTime.Now.AddDays(i),
                    EndDate = DateTime.Now.AddDays(i + 1)
                });
            }
            context.SaveChanges();

            Random rand = new Random();

            //  speakers
            for (int i = 1; i <= 10; i++)
            {
                context.Speakers.Add(new DAL.EF.Entities.Speaker()
                {
                    Name = "Speaker " + i,
                    Topic = "Topic " + rand.Next(1, 6), // Randomizing 
                    EventId = rand.Next(1, 6) // Assigning a random event from the 5 events created above
                });
            }
            context.SaveChanges();
             */
        }

    }

}
