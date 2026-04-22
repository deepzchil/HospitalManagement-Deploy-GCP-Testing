using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= GET: CREATE =================
        public IActionResult Create()
        {
            var slots = new List<string>
            {
                "09:00 AM",
                "10:00 AM",
                "11:00 AM",
                "12:00 PM",
                "02:00 PM",
                "03:00 PM",
                "04:00 PM",
                "05:00 PM"
            };

            ViewBag.Slots = slots;

            return View();
        }

        // ================= POST: CREATE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            // ✅ Prevent double booking
            var exists = _context.Appointments.Any(a =>
                a.DoctorName == appointment.DoctorName &&
                a.AppointmentDate.Date == appointment.AppointmentDate.Date &&
                a.TimeSlot == appointment.TimeSlot &&
                a.Status == "Booked");

            if (exists)
            {
                ModelState.AddModelError("", "This slot is already booked.");
                ViewBag.Slots = GetSlots();
                return View(appointment);
            }

            if (ModelState.IsValid)
            {
                appointment.Status = "Booked";

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Slots = GetSlots();
            return View(appointment);
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appointments.ToListAsync());
        }

        // ================= REUSABLE SLOT METHOD =================
        private List<string> GetSlots()
        {
            return new List<string>
            {
                "09:00 AM",
                "10:00 AM",
                "11:00 AM",
                "12:00 PM",
                "02:00 PM",
                "03:00 PM",
                "04:00 PM",
                "05:00 PM"
            };
        }
    }
}