using AutoMapper;
using BAL.DTOs;
using DAL.EF.Entities;
using DAL;
using System;
using System.Web;

public class AuthService
{
    static Mapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Token, TokenDTO>();
        });
        return new Mapper(config);
    }

    public static TokenDTO Authenticate(string uname, string pass)
    {
        var user = DataAccessFactory.AuthData().Authenticate(uname, pass);

        if (user != null && user.IsVerified)
        {
            Token t = new Token
            {
                CreatedAt = DateTime.Now,
                Key = Guid.NewGuid().ToString(),
                Uname = uname
            };
            var token = DataAccessFactory.TokenData().Create(t);
            return GetMapper().Map<TokenDTO>(token);
        }

        return null;
    }

    public static bool LogoutToken(string key)
    {
        if (DataAccessFactory.TokenData().Get(key) != null)
        {
            Token token = new Token
            {
                Key = key,
                ExpiredAt = DateTime.Now
            };
            var ret = DataAccessFactory.TokenData().Update(token);
            return ret != null;
        }

        return false;
    }

    public static bool IsTokenValid(string key)
    {
        var token = DataAccessFactory.TokenData().Get(key);
        return token != null && token.ExpiredAt == null;
    }

    public static bool IsTokenValidAdmin(string key)
    {
        var token = DataAccessFactory.TokenData().Get(key);
        return token != null && token.ExpiredAt == null && token.User.Role.Equals("Admin");
    }

    public static bool IsTokenValidUser(string key)
    {
        var token = DataAccessFactory.TokenData().Get(key);
        return token != null && token.ExpiredAt == null && token.User.Role.Equals("User");
    }

    //  logged-in user's username from the token
    public static string GetLoggedInUserName()
    {
        //  from the Authorization header
        var authHeader = HttpContext.Current.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader))
        {
            return null;
        }
        var tokenKey = authHeader.Trim();

        if (string.IsNullOrEmpty(tokenKey))
        {
            return null;
        }

        //  he token is valid
        var token = DataAccessFactory.TokenData().Get(tokenKey);
        if (token == null || token.ExpiredAt != null)
        {
            return null; // Token is invalid or expired
        }

        // Return the username associated with the token
        return token.Uname;
    }
}
