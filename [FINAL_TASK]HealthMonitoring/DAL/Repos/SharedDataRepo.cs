using DAL.EF.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    internal class SharedDataRepo : Repo, ISharedDataFeatures
    {
        public bool ShareData(string username, string sharedWith, DateTime expiryDate, string token)
        {
            var sharedData = new SharedData
            {
                Username = username,
                SharedWith = sharedWith,
                Token = token,
                CreatedAt = DateTime.Now,
                ExpiryDate = expiryDate,
                IsRevoked = false
            };
            db.SharedDatas.Add(sharedData);
            return db.SaveChanges() > 0;
        }

        public List<SharedData> GetSharedData(string sharedWith)
        {
            return db.SharedDatas
                     .Where(sd => sd.SharedWith == sharedWith && sd.ExpiryDate > DateTime.Now && !sd.IsRevoked)
                     .ToList();
        }

        public bool RevokeAccess(int id)
        {
            var sharedData = db.SharedDatas.Find(id);
            if (sharedData != null)
            {
                sharedData.IsRevoked = true;
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public bool ValidateToken(string token)
        {
            var sharedData = db.SharedDatas.FirstOrDefault(sd => sd.Token == token && !sd.IsRevoked && sd.ExpiryDate > DateTime.Now);
            return sharedData != null;
        }
    }
}
