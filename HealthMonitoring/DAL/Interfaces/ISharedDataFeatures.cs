using DAL.EF.Entities;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface ISharedDataFeatures
    {
        bool ShareData(string username, string sharedWith, DateTime expiryDate, string token);
        List<SharedData> GetSharedData(string sharedWith);
        bool RevokeAccess(int id);
        bool ValidateToken(string token);
    }
}
