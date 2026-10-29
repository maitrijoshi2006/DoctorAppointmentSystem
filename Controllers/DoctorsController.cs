using DoctorAppointmentSystem.Data;
using DoctorAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DoctorAppointmentSystem.Controllers;
[Authorize(Roles = "Admin")]
public class DoctorsController : Controller
{
    private readonly ApplicationDbContext _db;
    public DoctorsController(ApplicationDbContext db) => _db = db;
    public async Task<IActionResult> Index() => View(await _db.Doctors.OrderBy(d => d.FullName).ToListAsync());
    public IActionResult Create() => View(new Doctor());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(Doctor doctor) { if (!ModelState.IsValid) return View(doctor); _db.Doctors.Add(doctor); await _db.SaveChangesAsync(); TempData["Message"] = "Doctor added."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int id) { var d = await _db.Doctors.FindAsync(id); return d == null ? NotFound() : View(d); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, Doctor doctor) { if (id != doctor.Id) return BadRequest(); if (!ModelState.IsValid) return View(doctor); _db.Update(doctor); await _db.SaveChangesAsync(); TempData["Message"] = "Doctor updated."; return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { var d = await _db.Doctors.FindAsync(id); if (d == null) return NotFound(); if (await _db.Appointments.AnyAsync(a => a.DoctorId == id)) { TempData["Error"] = "This doctor has appointments. Mark the doctor unavailable instead."; return RedirectToAction(nameof(Index)); } _db.Doctors.Remove(d); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
}
