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
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Billing
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bills.ToListAsync());
        }

        // GET: Billing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var bill = await _context.Bills
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // GET: Billing/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Billing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PatientName,DoctorName,ConsultationFee,MedicineFee,RoomCharge,Tax,Discount,PaymentStatus")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        // GET: Billing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // POST: Billing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientName,DoctorName,ConsultationFee,MedicineFee,RoomCharge,Tax,Discount,PaymentStatus")] Bill bill)
        {
            if (id != bill.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }

        // GET: Billing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var bill = await _context.Bills
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // POST: Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill != null)
                _context.Bills.Remove(bill);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ================= PDF DOWNLOAD =================
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var bill = await _context.Bills.FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
                return NotFound();

            return new ViewAsPdf("Details", bill)
            {
                FileName = "Bill.pdf"
            };
        }

        // ================= HELPER =================
        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.Id == id);
        }
    }
}