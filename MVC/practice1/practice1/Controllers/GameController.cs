using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace practice1.Controllers
{
    public class GameController : Controller
    {
       /*
       public ActionResult Name(string name)
       {
           return Content ("welcome to " + name);
       }
       */
        public string Name(string name)
       {
           return "welcome to "+ name;
       }

        /*
         public ActionResult FullName(string fname, string lname)
         {
             return Content ( "welcome to " + fname + " " + lname);
         }
        */

        /*
         public ActionResult FullName(string first, string last)
            {
                if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(last))
                {
                    return Content("Your first name is = " + first + " and last name is = " + last);
                }
                else if (!string.IsNullOrEmpty(first))
                {
                    return Content("Your first name is = " + first);
                }
                else if (!string.IsNullOrEmpty(last))
                {
                    return Content("Your last name is = " + last);
                }
                else
                {
                    return Content("No name provided.");
                }
            }
         */

        public string FullName(string first, string last)
        {
            if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(last))
            {
                return "Your first name is = " + first + " and last name is = " + last;
            }
            else if (!string.IsNullOrEmpty(first))
            {
                return "Your first name is = " + first;
            }
            else if (!string.IsNullOrEmpty(last))
            {
                return "Your last name is = " + last;
            }
            else
            {
                return "No name provided.";
            }
        }


    }
}