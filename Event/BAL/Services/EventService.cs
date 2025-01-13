using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class EventService
    {
        // Initialize AutoMapper configuration for Event and EventDTO mapping
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Event, EventDTO>();  // Map Event to EventDTO
                cfg.CreateMap<EventDTO, Event>();  // Map EventDTO back to Event if needed
            });
            return new Mapper(config);
        }

        // Get all events as EventDTOs
        public static List<EventDTO> Get()
        {
            var repo = DataAccess.EventData();  // Get EventRepo instance
            var events = repo.Get();  // Get all events from the repository
            return GetMapper().Map<List<EventDTO>>(events);  // Map to EventDTO list
        }

        // Get an event by id as EventDTO
        public static EventDTO Get(int id)
        {
            var repo = DataAccess.EventData();  // Get EventRepo instance
            var eventEntity = repo.Get(id);  // Get the event by id
            return GetMapper().Map<EventDTO>(eventEntity);  // Map to EventDTO
        }

        // Get event with products (if applicable) and return EventDTO
        public static EventDTO GetWithProducts(int id)
        {
            var repo = DataAccess.EventData();  // Get EventRepo instance
            var eventEntity = repo.Get(id);  // Get event by id
            return GetMapper().Map<EventDTO>(eventEntity);  // Map to EventDTO (you may want to modify to include products if needed)
        }
        public static List<EventDTO> GetUpcomingEvents(DateTime date)
        {
            var repo = DataAccess.EventData();
            var upcomingEvents = repo.GetUpcomingEvents(date);  // Call the EventRepo method
            return GetMapper().Map<List<EventDTO>>(upcomingEvents);  // Map the result to EventDTO
        }

        // Search events by name
        public static List<EventDTO> SearchEventsByName(string name)
        {
            var repo = DataAccess.EventData();
            var events = repo.SearchEventsByName(name);  // Call the EventRepo method
            return GetMapper().Map<List<EventDTO>>(events);  // Map the result to EventDTO
        }

    }
}
