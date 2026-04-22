using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Data;

public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.PatientCount = _context.Patients.Count();
        ViewBag.DoctorCount = _context.Doctors.Count();
        ViewBag.AppointmentCount =
            _context.Appointments.Count(a =>
                a.AppointmentDate.Date == DateTime.Today);

        ViewBag.TotalBills =
            _context.Bills.Sum(b =>
                b.ConsultationFee +
                b.MedicineFee +
                b.RoomCharge +
                b.Tax -
                b.Discount);

        return View();
    }
}