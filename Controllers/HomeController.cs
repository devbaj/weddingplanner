using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weddingplanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace weddingplanner.Controllers
{
    public class HomeController : Controller
    {
        private Context dbContext;
        public HomeController(Context context)
        {
            dbContext = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return Redirect("/");
            List<Event> AllEvents = dbContext.Events
                .Include(e => e.Guests)
                .ThenInclude(att => att.Guest)
                .ThenInclude(u => u.HostingList)
                .ToList();
            List<Wedding> AllWeddings = new List<Wedding>();
            User thisUser = dbContext.Users
                .SingleOrDefault(u =>
                    u.UserID == HttpContext.Session.GetInt32("user"));
            foreach (Event e in AllEvents)
            {
                Wedding thisWedding = new Wedding();
                thisWedding.WeddingID = e.EventID;
                thisWedding.Couple = $"{e.Name1} & {e.Name2}";
                thisWedding.Date = e.Date;
                thisWedding.Address = e.Address;
                thisWedding.Guests = new List<User>();
                if (e.Host == thisUser)
                    thisWedding.IsHosting = true;
                else
                    thisWedding.IsHosting = false;
                bool found = false;
                foreach (Attendance a in e.Guests)
                {
                    thisWedding.Guests.Add(a.Guest);
                }
                foreach (User g in thisWedding.Guests)
                {
                    if (g.UserID == thisUser.UserID)
                    {
                        thisWedding.IsAttending = true;
                        found = true;
                    }
                }
                if (!found)
                    thisWedding.IsAttending = false;
                AllWeddings.Add(thisWedding);
            }
            return View(AllWeddings);
        }

        [HttpGet("wedding/{id}")]
        public IActionResult WeddingInfo(int id)
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout", "User");
            Event thisEvent = dbContext.Events
                .Include(e => e.Guests)
                .ThenInclude(att => att.Guest)
                .SingleOrDefault(e => e.EventID == id);
            Wedding thisWedding = new Wedding();
            thisWedding.Couple = $"{thisEvent.Name1} & {thisEvent.Name2}";
            thisWedding.Date = thisEvent.Date;
            string[] Address = thisEvent.Address.Split(" ");
            string AddressURLEscaped = string.Join("+", Address);
            thisWedding.Address = 
                "https://maps.googleapis.com/maps/api/staticmap?" +
                "&size=600x400" +
                $"&markers={AddressURLEscaped}" +
                "&key=AIzaSyDJPcoUQGa4y5WZMVTWAm9u_GJ2xv9_JaY";
            thisWedding.Guests = new List<User>();
            foreach (Attendance g in thisEvent.Guests)
                thisWedding.Guests.Add(g.Guest);

            return View("Wedding", thisWedding);
        }

        [HttpGet("wedding/add")]
        public IActionResult WeddingForm()
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout", "User");
            return View();
        }

        [HttpPost("wedding/add")]
        public IActionResult EventToDB(EventForm formData)
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout","User");
            if (!ModelState.IsValid)
                return View("WeddingForm");
            User host = dbContext.Users
                .SingleOrDefault(u =>
                    u.UserID == HttpContext.Session.GetInt32("user"));
            Event newEvent = new Event();
            newEvent.Name1 = formData.Name1;
            newEvent.Name2 = formData.Name2;
            newEvent.Date = formData.Date;
            newEvent.Address = formData.Address;
            newEvent.CreatedAt = DateTime.Now;
            newEvent.UpdatedAt = DateTime.Now;
            newEvent.UserID = host.UserID;
            dbContext.Events.Add(newEvent);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete/{id}")]
        public IActionResult DestroyWedding(int id)
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout", "User");
            Event thisEvent = dbContext.Events
                .SingleOrDefault(e => e.EventID == id);
            dbContext.Events.Remove(thisEvent);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("join/{id}")]
        public IActionResult JoinEvent(int id)
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout");
            User thisUser = dbContext.Users
                .SingleOrDefault(u =>
                    u.UserID == HttpContext.Session.GetInt32("user"));
            Event thisEvent = dbContext.Events
                .SingleOrDefault(e => e.EventID == id);
            Attendance newAtt = new Attendance();
            newAtt.Event = thisEvent;
            newAtt.Guest = thisUser;
            dbContext.Attendances.Add(newAtt);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{id}")]
        public IActionResult LeaveEvent(int id)
        {
            if (!HttpContext.Session.Keys.Contains("user"))
                return RedirectToAction("Logout", "User");
            Attendance cancellation = dbContext.Attendances
                .SingleOrDefault(a =>
                    a.UserID == HttpContext.Session.GetInt32("user")
                        && a.EventID == id);
            dbContext.Attendances.Remove(cancellation);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
