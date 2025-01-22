using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IEventRepo : IRepo<Event, int, string>
    {
        List<Event> GetUpcomingEvents(DateTime date); // Custom method 
        List<Event> SearchEventsByName(string name); // Custom method 
    }
}
