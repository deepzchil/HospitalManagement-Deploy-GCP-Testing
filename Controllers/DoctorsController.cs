using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= DOCTOR DASHBOARD =================
        public async Task<IActionResult> Index()
        {
            var today = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == DateTime.Today)
                .ToListAsync();

            return View(today);
        }
    }
}