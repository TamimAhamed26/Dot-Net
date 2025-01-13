using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF.Entities;
using DAL.Interface;


namespace DAL.Repos
{
    public class Repo<CLASS, ID, RET> : IRepo<CLASS, ID, RET> where CLASS : class
    {
        public LayerContext db;

        public Repo(LayerContext context)
        {
            db = context;
        }

        public RET Create(CLASS obj)
        {
            db.Set<CLASS>().Add(obj);
            db.SaveChanges();
            return (RET)Convert.ChangeType("Entity created successfully", typeof(RET));
        }

        public RET Update(CLASS obj)
        {
            var entry = db.Entry(obj);
            entry.State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return (RET)Convert.ChangeType("Entity updated successfully", typeof(RET));
        }


        public CLASS Get(ID id)
        {
            return db.Set<CLASS>().Find(id);
        }

        public List<CLASS> Get()
        {
            return db.Set<CLASS>().ToList();
        }

        public bool Delete(ID id)
        {
            var entity = db.Set<CLASS>().Find(id);
            if (entity != null)
            {
                db.Set<CLASS>().Remove(entity);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
