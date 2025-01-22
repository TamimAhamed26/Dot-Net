using demo.DTO;
using demo.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;

namespace demo.Controllers
{
    public class ChannelController : Controller
    {
        private readonly DemoEntities1 rw = new DemoEntities1();

        public static Channel Convert(ChannelDTO d)
        {
            return new Channel()
            {
                ChannelId = d.ChannelId,
                ChannelName = d.ChannelName,
                EstablishedYear = d.EstablishedYear,
                Country = d.Country
            };
        }
        public static ChannelDTO Convert(Channel d)
        {
            return new ChannelDTO()
            {
                ChannelId = d.ChannelId,
                ChannelName = d.ChannelName,
                EstablishedYear = d.EstablishedYear,
                Country = d.Country
            };
        }


        public List<ChannelDTO> Convert(List<Channel> data)
        {
            var list = new List<ChannelDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }
        //to show the list
        public ActionResult List()
        {
            var data = rw.Channels.ToList();//rw.Channel.ToList() is wrong
            return View(Convert(data));
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var exobj = rw.Channels.Find(Id);
            if (exobj == null) return HttpNotFound();
            return View(Convert(exobj));
        }

        [HttpPost]
        public ActionResult Edit(ChannelDTO channelDTO)
        {
            if (ModelState.IsValid)
            {
                //annotation method requires db context
                var duplicateChannel = rw.Channels
                    .FirstOrDefault(c => c.ChannelName == channelDTO.ChannelName && c.ChannelId != channelDTO.ChannelId);

                if (duplicateChannel != null)
                {

                    ModelState.AddModelError("ChannelName", "ChannelName must be unique.");
                    return View(channelDTO); 
                }

                var existingChannel = rw.Channels.Find(channelDTO.ChannelId);
                if (existingChannel == null)
                {
                    return RedirectToAction("List");
                }

                // Update the channel details

                rw.Entry(existingChannel).CurrentValues.SetValues(channelDTO);
                rw.SaveChanges();

                // success feedback
                TempData["Success"] = $"Channel '{channelDTO.ChannelName}' has been updated successfully.";
                return RedirectToAction("List");
            }

            // validation failure feedback
            TempData["Error"] = "Failed to update the channel. Please correct the errors and try again.";
            return View(channelDTO);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var Channel = rw.Channels.Find(id);
            if (Channel == null)
            {
                TempData["Msg"] = "Customer with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var ChannelDTO = new ChannelDTO
            {
                ChannelId = Channel.ChannelId,
                ChannelName = Channel.ChannelName,
                EstablishedYear = Channel.EstablishedYear,
                Country = Channel.Country,
            };

            return View(ChannelDTO);

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ChannelDTO());
        }
        [HttpPost]
        public ActionResult Create(ChannelDTO d)
        {
            if (ModelState.IsValid)
            {
                //  duplicate channel name
                var duplicateChannel = rw.Channels.FirstOrDefault(c => c.ChannelName == d.ChannelName);

                if (duplicateChannel != null)
                {
                    ModelState.AddModelError("ChannelName", "ChannelName already exists.");
                    return View(d); // Stay on the page for correction
                }

                // Add new channel
                var data = new Channel()
                {
                    ChannelName = d.ChannelName,
                    EstablishedYear = d.EstablishedYear,
                    Country = d.Country,
                };
                rw.Channels.Add(data);
                rw.SaveChanges();

                // Provide success feedback
                TempData["ISuccess"] = $"Channel '{d.ChannelName}' has been added successfully.";
                return RedirectToAction("List");
            }

            // Provide validation failure feedback
            TempData["IError"] = "Failed to add the channel. Please correct the errors and try again.";
            return View(d);
        }

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








    }
}