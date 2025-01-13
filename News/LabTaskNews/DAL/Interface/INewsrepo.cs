using DAL.EF.Entities;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface INewsRepo
    {
        string Create(News news);
        string Update(News news);
        News GetNews(int id);
        List<News> Get();
        bool Delete(int id);
        List<News> Search(string title = null, string category = null, DateTime? date = null);
    }
}
