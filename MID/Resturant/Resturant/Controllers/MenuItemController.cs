using Resturant.DTO;
using Resturant.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;


namespace Resturant.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ResturantEntities2 rw = new ResturantEntities2();

        public static MenuItem Convert(MenuItemDTO d)
        {
            return new MenuItem()
            {
                MenuId = d.MenuId,
                ItemId = d.ItemId,
                Availability = d.Availability,
                Description = d.Description,
                Price = d.Price,
                Name = d.Name,


            };
        }
        public static MenuItemDTO Convert(MenuItem d)
        {
            return new MenuItemDTO()
            {
                MenuId = d.MenuId,
                ItemId = d.ItemId,
                Availability = d.Availability,
                Description = d.Description,
                Price = d.Price,
                Name = d.Name,
            };
        }


        public List<MenuItemDTO> Convert(List<MenuItem> data)
        {
            var list = new List<MenuItemDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }
        //to show the list
        public ActionResult List()
        {
            var data = rw.MenuItems.ToList();//rw.Channel.ToList() is wrong
            return View(Convert(data));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var MenuItem = rw.MenuItems.Find(id);
            if (MenuItem == null)
            {
                TempData["Msg"] = "MenuItemDTO with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var MenuItemDTO = new MenuItemDTO
            {
                MenuId = MenuItem.MenuId,
                ItemId = MenuItem.ItemId,
                Availability = MenuItem.Availability,
                Description = MenuItem.Description,
                Price = MenuItem.Price,
                Name = MenuItem.Name,
            };

            return View(MenuItemDTO);

        }





     
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var exobj = rw.MenuItems.Find(Id);
            if (exobj == null) return HttpNotFound();
            return View(Convert(exobj));
        }

        [HttpPost]
        public ActionResult Edit(MenuItemDTO menuItemDTO)
        {
            if (ModelState.IsValid)
            {
                //annotation method requires db context
                var duplicateChannel = rw.MenuItems
                    .FirstOrDefault(c => c.Name == menuItemDTO.Name && c.ItemId != menuItemDTO.ItemId);

                if (duplicateChannel != null)
                {

                    ModelState.AddModelError("ChannelName", "ChannelName must be unique.");
                    return View(menuItemDTO);
                }

                var existingChannel = rw.MenuItems.Find(menuItemDTO.MenuId);
                if (existingChannel == null)
                {
                    return RedirectToAction("List");
                }

                // Update the channel details

                rw.Entry(existingChannel).CurrentValues.SetValues(menuItemDTO);
                rw.SaveChanges();

                // success feedback
                TempData["Success"] = $"Channel '{menuItemDTO.Name}' has been updated successfully.";
                return RedirectToAction("List");
            }

            // validation failure feedback
            TempData["Error"] = "Failed to update the channel. Please correct the errors and try again.";
            return View(menuItemDTO);
        }

     

     [HttpGet]
     public ActionResult Create()
     {
         return View(new MenuItemDTO());
     }
     [HttpPost]
     public ActionResult Create(MenuItemDTO d)
     {
         if (ModelState.IsValid)
         {
             //  duplicate channel name
             var duplicateChannel = rw.MenuItems.FirstOrDefault(c => c.Name == d.Name);

             if (duplicateChannel != null)
             {
                 ModelState.AddModelError("Name", "ChannelName already exists.");
                 return View(d); // Stay on the page for correction
             }

                // Add new channel
             var data = new MenuItem()
             {
                 Name = d.Name,
                 ItemId = d.ItemId,
                 Availability = d.Availability,
                 Description = d.Description,
                 Price=d.Price,

             };
             rw.MenuItems.Add(data);
             rw.SaveChanges();

             // Provide success feedback
             TempData["ISuccess"] = $"Channel '{d.Name}' has been added successfully.";
             return RedirectToAction("List");
         }

         // Provide validation failure feedback
         TempData["IError"] = "Failed to add the channel. Please correct the errors and try again.";
         return View(d);
     }
        /*
          public ActionResult ViewProgramsGroupedByChannels(int id)
          {
              // Fetch the specific channel with its associated programs
              var channel = rw.Channels.Include("Programs").FirstOrDefault(c => c.ChannelId == id);

              if (channel == null)
              {
                  return HttpNotFound();
              }

              // Pass only the selected channel to the view
              return View(channel);
          }

          [HttpGet]
          public ActionResult Delete(int id)
          {
              var channel = rw.Channels.Find(id);

              if (channel == null)
              {
                  return HttpNotFound();
              }

              // Check if there are any associated programs
              if (channel.Programs.Any())
              {
                  TempData["ErrorMessage"] = "Cannot delete this channel as it has associated programs.";
                  return RedirectToAction("ViewProgramsGroupedByChannels", new { id = channel.ChannelId });
              }

              return View(channel);
          }


          [HttpPost]
          public ActionResult DeleteChannel(int channelId)
          {
              var channel = rw.Channels.Find(channelId);
              if (channel == null)
              {
                  return HttpNotFound();
              }

              try
              {
                  rw.Channels.Remove(channel);
                  rw.SaveChanges();
                  TempData["SuccessMessage"] = "Channel deleted successfully.";
              }
              catch (Exception)
              {
                  TempData["ErrorMessage"] = "An error occurred while deleting the channel.";

              }

              return RedirectToAction("List");
          }


          */





    }
}