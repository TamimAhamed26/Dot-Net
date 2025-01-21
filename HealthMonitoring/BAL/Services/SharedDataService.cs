using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using System;
using System.Collections.Generic;

namespace BAL.Services
{
    public class SharedDataService
    {
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SharedData, SharedDataDTO>();
                cfg.CreateMap<SharedDataDTO, SharedData>();
            });
            return new Mapper(config);
        }

        // Share health data with a healthcare provider
        public static bool ShareData(string username, string sharedWith, DateTime expiryDate)
        {
            // Generate a unique token
            var token = Guid.NewGuid().ToString();

            // Use the repository to save the shared data
            return DataAccessFactory.SharedDataFeatures().ShareData(username, sharedWith, expiryDate, token);
        }

        // Get shared health data for a specific healthcare provider
        public static List<SharedDataDTO> GetSharedData(string sharedWith)
        {
            var sharedData = DataAccessFactory.SharedDataFeatures().GetSharedData(sharedWith);
            return GetMapper().Map<List<SharedDataDTO>>(sharedData);
        }

        // Revoke access to shared health data
        public static bool RevokeAccess(int id)
        {
            return DataAccessFactory.SharedDataFeatures().RevokeAccess(id);
        }

        // Validate a token to check if the shared data is accessible
        public static bool ValidateToken(string token)
        {
            return DataAccessFactory.SharedDataFeatures().ValidateToken(token);
        }
    }
}
