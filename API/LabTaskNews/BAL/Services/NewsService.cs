using System;
using System.Collections.Generic;
using AutoMapper;
using BAL.DTOs;
using DAL.EF.Entities;
using DAL.Repos;

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
               return _mapper.Map<List<NewsDTO>>(new NewsRepo().Get());
           }

           public static NewsDTO GetNewsById(int id)
           {
               return _mapper.Map<NewsDTO>(new NewsRepo().GetNews(id));
           }

           public static string CreateNews(NewsDTO newsDTO)
           {
               var mapper = _mapper;
               var news = mapper.Map<News>(newsDTO);

               var repo = new NewsRepo();
               return repo.Create(news); 
           }

           public static string UpdateNews(NewsDTO newsDTO)
           {
               var mapper = _mapper;
               var news = mapper.Map<News>(newsDTO);

               var repo = new NewsRepo();
               return repo.Update(news); 
           }

           public static bool DeleteNews(int id)
           {
               var repo = new NewsRepo();
               return repo.Delete(id); 
           }
        public static List<NewsDTO> SearchNews(string title = null, string category = null, DateTime? date = null)
        {
            var repo = new NewsRepo();

            var filteredNews = repo.Search(title, category, date);

            return _mapper.Map<List<NewsDTO>>(filteredNews);
        }



    }


}
