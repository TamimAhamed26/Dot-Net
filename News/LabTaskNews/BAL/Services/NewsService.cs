using System;
using System.Collections.Generic;
using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using DAL.Interfaces;

namespace BAL.Services
{
    public class NewsService
    {
        private static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<News, NewsDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        private static readonly IMapper _mapper = InitializeMapper();

        public static List<NewsDTO> Get()
        {
            var repo = DataAccess.NewsData(); 
            return _mapper.Map<List<NewsDTO>>(repo.Get());
        }

        public static NewsDTO GetNewsById(int id)
        {
            var repo = DataAccess.NewsData(); 
            return _mapper.Map<NewsDTO>(repo.GetNews(id));
        }

        public static string CreateNews(NewsDTO newsDTO)
        {
            var news = _mapper.Map<News>(newsDTO);
            var repo = DataAccess.NewsData(); 
            return repo.Create(news);
        }

        public static string UpdateNews(NewsDTO newsDTO)
        {
            var news = _mapper.Map<News>(newsDTO);
            var repo = DataAccess.NewsData();
            return repo.Update(news); 
        }


        public static bool DeleteNews(int id)
        {
            var repo = DataAccess.NewsData(); 
            return repo.Delete(id);
        }

        public static List<NewsDTO> SearchNews(string title = null, string category = null, DateTime? date = null)
        {
            var repo = DataAccess.NewsData();
            var filteredNews = repo.Search(title, category, date);
            return _mapper.Map<List<NewsDTO>>(filteredNews);
        }
    }
}
