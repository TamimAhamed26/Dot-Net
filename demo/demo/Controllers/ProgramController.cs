using demo.DTO;
using demo.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demo.Controllers
{
    public class ProgramController : Controller
    {
        // GET: Program
       private readonly DemoEntities1 rw = new DemoEntities1();

        public static Program Convert(ProgramDTO d)
        {
            return new Program()
            {
                ProgramId = d.ProgramId,
                ProgramName = d.ProgramName,
                TRPScore = d.TRPScore,
                ChannelId = d.ChannelId,
                AirTime=d.AirTime,
            };
        }
        public static ProgramDTO Convert(Program d)
        {
            return new ProgramDTO()
            {
                ProgramId = d.ProgramId,
                ProgramName = d.ProgramName,
                TRPScore = d.TRPScore,
                ChannelId = d.ChannelId,
                AirTime = d.AirTime,
            };
        }


        public List<ProgramDTO> Convert(List<Program> data)
        {
            var list = new List<ProgramDTO>();
            foreach (var d in data)
            {
                list.Add(Convert(d));
            }
            return list;
        }

        //to show the list
        public ActionResult List(string searchTerm, decimal? minTRP, decimal? maxTRP, int? channelId, string sortOrder)
        {
            var query = rw.Programs.Include("Channel").AsQueryable();

            // Search filter: partial match
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.ProgramName.Contains(searchTerm));
            }

            // TRP range filter
            if (minTRP.HasValue)
            {
                query = query.Where(p => p.TRPScore >= minTRP.Value);
            }
            if (maxTRP.HasValue)
            {
                query = query.Where(p => p.TRPScore <= maxTRP.Value);
            }

            // Channel filter
            if (channelId.HasValue)
            {
                query = query.Where(p => p.ChannelId == channelId.Value);
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "ChannelNameDesc":
                    query = query.OrderByDescending(p => p.Channel.ChannelName);
                    break;
                default:
                    query = query.OrderBy(p => p.Channel.ChannelName);
                    break;
            }

            var data = query.ToList();

            // Channel list for dropdown
            ViewBag.Channels = new SelectList(rw.Channels.ToList(), "ChannelId", "ChannelName", channelId);

            // Sort order parameter in ViewBag
            ViewBag.CurrentSort = sortOrder;

            // Search parameters in ViewBag
            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinTRP = minTRP;
            ViewBag.MaxTRP = maxTRP;
            ViewBag.SelectedChannelId = channelId;

            return View(Convert(data));
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var Program = rw.Programs.Find(id);
            if (Program == null)
            {
                TempData["Msg"] = "Customer with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var ProgramDTO = new ProgramDTO
            {
                ProgramId = Program.ProgramId,
                ProgramName = Program.ProgramName,
                TRPScore = Program.TRPScore,
                ChannelId = Program.ChannelId,
                AirTime = Program.AirTime,
            };

            return View(ProgramDTO);

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //user is logged in
            var user = Session["user"] as User;
            if (user == null)
            {
                TempData["Error"] = "Please log in to edit the program.";
                return RedirectToAction("List", "Program");
            }

            // edit using the id
            var program = rw.Programs.Find(id);
            if (program == null)
            {
                TempData["Error"] = "Program not found.";
                return RedirectToAction("List");
            }

          
            var programDTO = new ProgramDTO
            {
                ProgramId = program.ProgramId,
                ProgramName = program.ProgramName,
                TRPScore = program.TRPScore,
                AirTime = program.AirTime,
                ChannelId = program.ChannelId
            };

         
            ViewBag.IsAdmin = user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            return View(programDTO);
        }


        [HttpPost]
        public ActionResult Edit(ProgramDTO programDTO)
        {
            // Check user session
            var user = Session["user"] as User;
            if (user == null)
            {
                TempData["Error"] = "Session expired. Please log in again.";
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                // Check for duplicate program (excluding the current one)
                var duplicateProgram = rw.Programs.FirstOrDefault(p =>
                    p.ProgramName == programDTO.ProgramName &&
                    p.ChannelId == programDTO.ChannelId &&
                    p.ProgramId != programDTO.ProgramId);

                if (duplicateProgram != null)
                {
                    ModelState.AddModelError("ProgramName", "The combination of Program Name and Channel must be unique.");

                    // Reload dropdown data (required for dropdowns on the edit view)
                    ViewBag.Channels = rw.Channels.Select(c => new SelectListItem
                    {
                        Value = c.ChannelId.ToString(),
                        Text = c.ChannelName
                    }).ToList();

                    return View(programDTO);
                }

                // Fetch the existing program
                var existingProgram = rw.Programs.Find(programDTO.ProgramId);
                if (existingProgram == null)
                {
                    TempData["Error"] = "Program not found.";
                    return RedirectToAction("List");
                }

                // Update fields
                existingProgram.ProgramName = programDTO.ProgramName;
                existingProgram.AirTime = programDTO.AirTime;

                // Only allow admin to edit TRPScore
                if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    existingProgram.TRPScore = programDTO.TRPScore;
                }

                rw.SaveChanges();

                // Destroy session after successful edit
                Session["user"] = null;

                TempData["Success"] = "Program updated successfully.";
                return RedirectToAction("List");
            }

            // Reload dropdown data for dropdowns in case of error
            ViewBag.Channels = rw.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();

            TempData["Error"] = "Failed to update program. Please correct the errors.";
            return View(programDTO);
        }



        [HttpGet]
        public ActionResult Create()
        {
            // Fetch list of channels to populate the dropdown
            ViewBag.Channels = rw.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();

            return View(new ProgramDTO());
        }

        [HttpPost]
        public ActionResult Create(ProgramDTO d)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate program name within the same channel
                var duplicateProgram = rw.Programs.FirstOrDefault(p => p.ProgramName == d.ProgramName && p.ChannelId == d.ChannelId);

                if (duplicateProgram != null)
                {
                    ModelState.AddModelError("ProgramName", "ProgramName already exists for this channel.");
                    ViewBag.Channels = rw.Channels.Select(c => new SelectListItem
                    {
                        Value = c.ChannelId.ToString(),
                        Text = c.ChannelName
                    }).ToList();
                    return View(d);
                }

                // Add new program
                var data = new Program()
                {
                    ProgramName = d.ProgramName,
                    TRPScore = d.TRPScore,
                    AirTime = d.AirTime,
                    ChannelId = d.ChannelId
                };

                rw.Programs.Add(data);
                rw.SaveChanges();

                TempData["Success"] = $"Program '{d.ProgramName}' has been added successfully.";
                return RedirectToAction("List");
            }

            // Reload dropdown on validation failure
            ViewBag.Channels = rw.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();
            return View(d);
        }

        

    }
}