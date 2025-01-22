using Hotel.DTO;
using Hotel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hotel.Controllers
{
    public class BookingController : Controller
    {
        private readonly hotelEntities3 rw = new hotelEntities3();

        // Helper Methods for Conversion
        public static Booking Convert(BookingDTO dto)
        {
            return new Booking()
            {
                BookingID = dto.BookingID,
                RoomNumber = dto.RoomNumber,
                CustomerName = dto.CustomerName,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut
            };
        }

        public static BookingDTO Convert(Booking entity)
        {
            return new BookingDTO()
            {
                BookingID = entity.BookingID,
                RoomNumber = entity.RoomNumber,
                CustomerName = entity.CustomerName,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut
            };
        }

        public List<BookingDTO> Convert(List<Booking> entities)
        {
            var list = new List<BookingDTO>();
            foreach (var entity in entities)
            {
                list.Add(Convert(entity));
            }
            return list;
        }

        public ActionResult List(DateTime? startDate, DateTime? endDate)
        {
            var bookings = rw.Bookings.AsQueryable();

            // Apply filtering if date range is provided
            if (startDate.HasValue)
            {
                bookings = bookings.Where(b => b.CheckIn >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                bookings = bookings.Where(b => b.CheckOut <= endDate.Value);
            }

            // Convert the filtered data to DTOs and pass to the view
            var bookingDTOs = Convert(bookings.ToList());
            return View(bookingDTOs);
        }


        // Create a New Booking
        [HttpGet]
        public ActionResult Create()
        {
            var bookingDTO = new BookingDTO
            {
                CheckIn = DateTime.Now.Date,
                CheckOut = DateTime.Now.Date.AddDays(1) // Default to the next day
            };

            // Populate the dropdown with available rooms
            ViewBag.RoomNumbers = rw.Rooms
                .Select(r => new SelectListItem
                {
                    Value = r.RoomNumber.ToString(),
                    Text = $"Room {r.RoomNumber}"
                }).ToList();

            return View(bookingDTO);
        }




        [HttpPost]
        public ActionResult Create(BookingDTO dto)
        {
            if (ModelState.IsValid)
            {
                // Check if the RoomNumber exists
                var roomExists = rw.Rooms.Any(r => r.RoomNumber == dto.RoomNumber);
                if (!roomExists)
                {
                    TempData["ErrorMessage"] = "The specified room does not exist. Please select a valid room.";
                    return View(dto);
                }

                // Ensure CheckIn and CheckOut dates start from the next day
                if (dto.CheckIn.Date <= DateTime.Now.Date)
                {
                    TempData["ErrorMessage"] = "Check-in date must be at least one day in the future.";
                    return View(dto);
                }

                if (dto.CheckOut.Date <= dto.CheckIn.Date)
                {
                    TempData["ErrorMessage"] = "Check-out date must be after the check-in date.";
                    return View(dto);
                }

                // Check room availability
                var overlappingBookings = rw.Bookings.Any(b =>
                    b.RoomNumber == dto.RoomNumber &&
                    ((dto.CheckIn >= b.CheckIn && dto.CheckIn < b.CheckOut) || // Overlaps start
                     (dto.CheckOut > b.CheckIn && dto.CheckOut <= b.CheckOut) || // Overlaps end
                     (dto.CheckIn <= b.CheckIn && dto.CheckOut >= b.CheckOut))); // Encloses existing booking

                if (overlappingBookings)
                {
                    TempData["ErrorMessage"] = "The selected room is already booked during the specified dates. Please choose another room or modify the dates.";
                    return View(dto);
                }

                // Create the booking
                var booking = Convert(dto);
                rw.Bookings.Add(booking);
                rw.SaveChanges();

                TempData["SuccessMessage"] = $"Booking '{dto.BookingID}' has been created successfully.";
                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Failed to create the booking. Please correct the errors and try again.";
            return View(dto);
        }


        // Edit a Booking
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var booking = rw.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            return View(Convert(booking));
        }

        [HttpPost]
        public ActionResult Edit(BookingDTO dto)
        {
            if (ModelState.IsValid)
            {
                var existingBooking = rw.Bookings.Find(dto.BookingID);
                if (existingBooking == null) return RedirectToAction("List");

                rw.Entry(existingBooking).CurrentValues.SetValues(dto);
                rw.SaveChanges();

                TempData["SuccessMessage"] = $"Booking '{dto.BookingID}' has been updated successfully.";
                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Failed to update the booking. Please correct the errors and try again.";
            return View(dto);
        }

        // Delete a Booking
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var booking = rw.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            return View(Convert(booking));
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var booking = rw.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            try
            {
                rw.Bookings.Remove(booking);
                rw.SaveChanges();

                TempData["SuccessMessage"] = "Booking has been deleted successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the booking.";
            }

            return RedirectToAction("List");
        }

        // Booking Details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var booking = rw.Bookings.Find(id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction("List");
            }

            return View(Convert(booking));
        }
    }
}
