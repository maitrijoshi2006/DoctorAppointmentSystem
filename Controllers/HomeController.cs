using Microsoft.AspNetCore.Mvc;
using DoctorAppointmentSystem.Data;
using Microsoft.EntityFrameworkCore;
namespace DoctorAppointmentSystem.Controllers;
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;
    public async Task<IActionResult> Index() => View(await _db.Doctors.Where(d => d.IsAvailable).OrderBy(d => d.FullName).ToListAsync());
    public IActionResult Privacy() => View();
    public IActionResult Error() => View();
}
