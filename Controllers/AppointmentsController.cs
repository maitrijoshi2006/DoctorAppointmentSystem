using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Models;
using DoctorAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace DoctorAppointmentSystem.Controllers;
[Authorize]
public class AppointmentsController : Controller
{
    private readonly ApplicationDbContext _db;
    public AppointmentsController(ApplicationDbContext db) => _db = db;
    private async Task<BookAppointmentViewModel> Prepare(BookAppointmentViewModel model) { model.Doctors = await _db.Doctors.Where(d => d.IsAvailable).OrderBy(d => d.FullName).Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.FullName + " — " + d.Specialization }).ToListAsync(); return model; }
    public async Task<IActionResult> Index() { var q = _db.Appointments.Include(a => a.Doctor).Include(a => a.Patient).AsQueryable(); if (!User.IsInRole("Admin")) q = q.Where(a => a.PatientId == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value); return View(await q.OrderByDescending(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToListAsync()); }
    [HttpGet] public async Task<IActionResult> Book() => View(await Prepare(new BookAppointmentViewModel()));
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Book(BookAppointmentViewModel model)
    {
        if (model.AppointmentDate.Date < DateTime.Today) ModelState.AddModelError(nameof(model.AppointmentDate), "Choose today or a future date.");
        if (!TimeSpan.TryParse(model.AppointmentTime, out var time) || time < TimeSpan.FromHours(9) || time >= TimeSpan.FromHours(17) || time.Minutes % 30 != 0) ModelState.AddModelError(nameof(model.AppointmentTime), "Choose a 30-minute slot between 09:00 and 16:30.");
        var doctor = await _db.Doctors.FindAsync(model.DoctorId);
        if (doctor == null || !doctor.IsAvailable) ModelState.AddModelError(nameof(model.DoctorId), "Choose an available doctor.");
        if (doctor != null && await _db.Appointments.AnyAsync(a => a.DoctorId == model.DoctorId && a.AppointmentDate.Date == model.AppointmentDate.Date && a.AppointmentTime == model.AppointmentTime && a.Status != "Cancelled")) ModelState.AddModelError(nameof(model.AppointmentTime), "This slot is already booked. Please choose another time.");
        if (!ModelState.IsValid) { await Prepare(model); return View(model); }
        var appointment = new Appointment { PatientId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value, DoctorId = model.DoctorId, AppointmentDate = model.AppointmentDate.Date, AppointmentTime = model.AppointmentTime, Reason = model.Reason, Status = "Pending" };
        _db.Appointments.Add(appointment); await _db.SaveChangesAsync(); TempData["Message"] = "Appointment booked successfully."; return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Cancel(int id) { var a = await _db.Appointments.FindAsync(id); if (a == null) return NotFound(); if (!User.IsInRole("Admin") && a.PatientId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value) return Forbid(); if (a.Status != "Cancelled") { a.Status = "Cancelled"; await _db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> SetStatus(int id, string status) { if (!new[] { "Pending", "Confirmed", "Completed", "Cancelled" }.Contains(status)) return BadRequest(); var a = await _db.Appointments.FindAsync(id); if (a == null) return NotFound(); a.Status = status; await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
}
