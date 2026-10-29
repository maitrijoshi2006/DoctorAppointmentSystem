using DoctorAppointmentSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DoctorAppointmentSystem.Controllers;
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminController(ApplicationDbContext db) => _db = db;
    public async Task<IActionResult> Index() { ViewBag.Doctors = await _db.Doctors.CountAsync(); ViewBag.Patients = await _db.Users.CountAsync(u => u.UserName != "admin@clinic.local"); ViewBag.Appointments = await _db.Appointments.CountAsync(); ViewBag.Pending = await _db.Appointments.CountAsync(a => a.Status == "Pending"); ViewBag.Recent = await _db.Appointments.Include(a => a.Doctor).Include(a => a.Patient).OrderByDescending(a => a.CreatedAt).Take(8).ToListAsync(); return View(); }
}
