using Hotel.DTO;
using Hotel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hotel.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        private readonly hotelEntities3 rw = new hotelEntities3();

        public static Room Convert(RoomDTO d)
        {
            return new Room()
            {
                RoomNumber = d.RoomNumber,
                Type = d.Type,
                Price = d.Price,
                Availability = d.Availability
            };
        }
        public static RoomDTO Convert(Room d)
        {
            return new RoomDTO()
            {
                RoomNumber = d.RoomNumber,
                Type = d.Type,
                Price = d.Price,
                Availability = d.Availability
            };
        }


        public List<RoomDTO> Convert(List<Room> data)
        {
            var list = new List<RoomDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }
        //to show the list
        public ActionResult List()
        {
            var data = rw.Rooms.ToList();//rw.Channel.ToList() is wrong
            return View(Convert(data));
        }
    
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var exobj = rw.Rooms.Find(Id);
            if (exobj == null) return HttpNotFound();
            return View(Convert(exobj));
        }

        [HttpPost]
        public ActionResult Edit(RoomDTO roomsDTO)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing room from the database using the RoomNumber
                var existingRoom = rw.Rooms.Find(roomsDTO.RoomNumber);
                if (existingRoom == null)
                {
                    return RedirectToAction("List");
                }

                // Update the room details
                rw.Entry(existingRoom).CurrentValues.SetValues(roomsDTO);
                rw.SaveChanges();

                // Success feedback
                TempData["Success"] = $"Room '{roomsDTO.RoomNumber}' has been updated successfully.";
                return RedirectToAction("List");
            }

            // Validation failure feedback
            TempData["Error"] = "Failed to update the room. Please correct the errors and try again.";
            return View(roomsDTO);
        }



        [HttpGet]
        public ActionResult Details(int id)
        {
            var Room = rw.Rooms.Find(id);
            if (Room == null)
            {
                TempData["Msg"] = "Customer with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var RoomDTO = new RoomDTO
            {
                RoomNumber = Room.RoomNumber,
                Type = Room.Type,
                Price = Room.Price,
                Availability = Room.Availability
            };

            return View(RoomDTO);

        }
      
    [HttpGet]
    public ActionResult Create()
    {
        return View(new RoomDTO());
    }
    [HttpPost]
    public ActionResult Create(RoomDTO d)
    {
        if (ModelState.IsValid)
        {
            
            
            
            var data = new Room()
            {
                Type = d.Type,
                Price = d.Price,
                Availability = d.Availability,
            };
            rw.Rooms.Add(data);
            rw.SaveChanges();

            // Provide success feedback
            TempData["ISuccess"] = $"Channel '{d.RoomNumber}' has been added successfully.";
            return RedirectToAction("List");
        }

        // Provide validation failure feedback
        TempData["IError"] = "Failed to add the channel. Please correct the errors and try again.";
        return View(d);
    }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            // Fetch the Room entity from the database
            var room = rw.Rooms.Find(id);

            if (room == null)
            {
                return HttpNotFound();
            }

            // Map the Room entity to a RoomDTO object
            var roomDTO = new RoomDTO
            {
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                Price = room.Price,
                Availability = room.Availability
            };

            // Check if there are associated bookings
            var associatedBookings = rw.Bookings.Any(b => b.RoomNumber == id);

            if (associatedBookings)
            {
                ViewBag.ConfirmationMessage = "This room has associated bookings. Deleting the room will also delete these bookings. Are you sure you want to proceed?";
            }
            else
            {
                ViewBag.ConfirmationMessage = "Are you sure you want to delete this room?";
            }

            // Pass the RoomDTO object to the view
            return View(roomDTO);
        }


        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var room = rw.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            try
            {
     
                var associatedBookings = rw.Bookings.Where(b => b.RoomNumber == id).ToList();
                foreach (var booking in associatedBookings)
                {
                    rw.Bookings.Remove(booking);
                }

                // Delete the room
                rw.Rooms.Remove(room);
                rw.SaveChanges();

                TempData["SuccessMessage"] = "Room and its associated bookings (if any) have been deleted successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the room and its associated bookings.";
            }

            return RedirectToAction("List");
        }


    }
}