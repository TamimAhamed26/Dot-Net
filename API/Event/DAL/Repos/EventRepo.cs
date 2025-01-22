using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF.Entities;
using DAL.Interface;

namespace DAL.Repos
{
    public class EventRepo : Repo<Event, int, string>, IEventRepo
    {
        public EventRepo(LayerContext context) : base(context) { }

     
        public List<Event> GetUpcomingEvents(DateTime date)
        {
            return db.Events.Where(e => e.StartDate >= date)
                            .OrderBy(e => e.StartDate)
                            .ToList();
        }

       
        public List<Event> SearchEventsByName(string name)
        {
            return db.Events.Where(e => e.Name.ToLower().Contains(name.ToLower()))
                            .ToList();
        }
    }
}
