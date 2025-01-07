using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF.Entities;

namespace DAL.Repos
{
    public class NewsRepo : Repo
    {
        public List<News> Get()
        {

            return db.NewsS.ToList();
        }

        public News GetNews(int id)
        {

            return db.NewsS.Find(id);
        }

        public string Create(News obj)
        {
            try
            {
                db.NewsS.Add(obj);
                db.SaveChanges();
                return "Product added successfully!";
            }
            catch
            {
                return "Error occurred while adding product.";
            }
        }

        public string Update(News obj)
        {
            try
            {
                var existing = db.NewsS.Find(obj.Id);
                if (existing != null)
                {
                    db.Entry(existing).CurrentValues.SetValues(obj);
                    db.SaveChanges();
                    return "news updated successfully!";
                }
                return "news not found.";
            }
            catch
            {
                return "Error occurred while updating product.";
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var product = db.NewsS.Find(id);
                if (product != null)
                {
                    db.NewsS.Remove(product);
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

            
            if (!string.IsNullOrEmpty(title))
                query = query.Where(n => n.Title.Contains(title));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(n => n.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (date.HasValue)
                query = query.Where(n => n.PublishDate == date.Value);

          
            return query.ToList();
        }




    }



}
