using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF.Entities;
using DAL.Interfaces;

namespace DAL.Repos
{
    public class NewsRepo : Repo, INewsRepo
    {
        public List<News> Get()
        {
            return db.NewsS.ToList();
        }

        public News GetNews(int id)
        {
            return db.NewsS.Find(id);
        }

        public string Create(News news)
        {
            try
            {
                db.NewsS.Add(news);
                db.SaveChanges();
                return "News added successfully!";
            }
            catch (Exception ex)
            {
                return $"Error occurred while adding news: {ex.Message}";
            }
        }


        public string Update(News news)
        {
            try
            {
                var exobj = GetNews(news.Id);
                if (exobj != null)
                {
                    db.Entry(exobj).CurrentValues.SetValues(news);
                    db.SaveChanges();
                    return "News updated successfully!";
                }
                return "News not found.";
            }
            catch (Exception ex)
            {
                return $"Error occurred while updating news: {ex.Message}";
            }
        }




        public bool Delete(int id)
        {
            try
            {
                var news = db.NewsS.Find(id);
                if (news != null)
                {
                    db.NewsS.Remove(news);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<News> Search(string title = null, string category = null, DateTime? date = null)
        {
            var query = db.NewsS.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(n => n.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(n => n.Category.Equals(category));

            if (date.HasValue)
                query = query.Where(n => n.PublishDate.Date == date.Value.Date);

            return query.ToList();
        }
    }
}
