using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UserService
    {
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
            });
            return new Mapper(config);
        }
 

        public static bool RegisterUser(UserDTO obj)
        {
            var data = GetMapper().Map<User>(obj);
            data.IsVerified = false; 
            var createdUser = DataAccessFactory.UserData().Create(data);
            return createdUser != null;
        }
        public static bool VerifyUser(string username)
        {
            var userRepo = DataAccessFactory.UserData();
            var user = userRepo.Get(username);

            if (user == null) return false;

            user.IsVerified = true; 
            return true;
        }

        public static bool DenyUser(string username)
        {
            var userRepo = DataAccessFactory.UserData();
            var user = userRepo.Get(username);

            if (user == null) return false;

            return userRepo.Delete(username); 
        }
    }
}
