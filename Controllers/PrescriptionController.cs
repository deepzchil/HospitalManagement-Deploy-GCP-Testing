using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Rotativa.AspNetCore;

namespace HospitalManagementSystem.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prescription
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prescriptions.ToListAsync());
        }

        // GET: Prescription/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prescription == null) return NotFound();

            return View(prescription);
        }

        // GET: Prescription/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,PatientName,DoctorName,Diagnosis,Medicines,Dosage,Notes,FollowUpDate")]
            Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                prescription.CreatedDate = DateTime.Now;

                _context.Add(prescription);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(prescription);
        }

        // GET: Prescription/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription == null) return NotFound();

            return View(prescription);
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,PatientName,DoctorName,Diagnosis,Medicines,Dosage,Notes,CreatedDate,FollowUpDate")]
            Prescription prescription)
        {
            if (id != prescription.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Prescriptions.Any(e => e.Id == prescription.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(prescription);
        }

        // GET: Prescription/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prescription == null) return NotFound();

            return View(prescription);
        }

        // POST: Prescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ STEP 14.8 PDF DOWNLOAD (ADDED HERE)
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
                return NotFound();

            return new ViewAsPdf("Details", prescription)
            {
                FileName = "Prescription.pdf"
            };
        }
    }
}